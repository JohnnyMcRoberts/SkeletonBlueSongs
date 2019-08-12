namespace SongsBurger.Controllers
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using SongsDatabase.Databases;
    using SongsDatabase.DataModels;

    using SongsFileImportExport.Parser;

    using RequestsResponses;
    using Settings;
    using Utilities;

    [Route("api/[controller]")]
    [ApiController]
    public class DataModelController : ControllerBase
    {
        #region Private Data

        private readonly MongoDbSettings _dbConfig;
        private readonly FileUploadSettings _uploaderSettings;
        private readonly AlbumPlayedDatabase _albumPlayedDatabase;
        private readonly UserDatabase _userDatabase;

        private static readonly object Lock = new object();

        #endregion

        #region Utility Methods

        private void RemoveExistingItemsForUser(string userName)
        {
            lock (Lock)
            {
                if (_albumPlayedDatabase.LoadedItems.Any(x => x.UserName == userName))
                {
                    List<AlbumPlayed> itemsToRemove =
                        _albumPlayedDatabase.LoadedItems.Where(x => x.UserName == userName).ToList();

                    if (itemsToRemove.Any())
                    {
                        foreach (AlbumPlayed item in itemsToRemove)
                        {
                            _albumPlayedDatabase.RemoveItemFromDatabase(item);
                        }
                    }
                }
            }
        }

        private static void AddIfUnique(Dictionary<string, string> stringLookup, string location)
        {
            if (!stringLookup.ContainsKey(location))
                stringLookup.Add(location, location);
        }

        #endregion

        #region HTTP Requests

        [HttpGet("[action]")]
        public IEnumerable<DisplayAlbum> GetAllAlbumsPlayed()
        {
            var albums = new ObservableCollection<DisplayAlbum>();

            lock (Lock)
            {
                foreach (var albumPlayed in _albumPlayedDatabase.LoadedItems)
                {
                    albums.Add(new DisplayAlbum(albumPlayed));
                }
            }

            if (albums.Count == 0)
            {
                albums.Add(
                    new DisplayAlbum
                    {
                        Artist = "a",
                        Album = "b",
                        Date = DateTime.Now,
                        Location = "c",
                        UserName = "d"
                    });
            }

            return albums;
        }

        [HttpGet("[action]/{key}")]
        [ProducesResponseType(201, Type = typeof(SongsFilesDetailsResponse))]
        [ProducesResponseType(400)]
        public SongsFilesDetailsResponse GetAllAlbumsPlayedFromFile(string key)
        {
            SongsFilesDetailsResponse resp = new SongsFilesDetailsResponse
            {
                FileName = key,
                ErrorCode = (int)SongsFilesResponseCode.Success
            };

            string fullPath = 
                FileUtilities.GetFullSongsDetailsFilePath(key, _uploaderSettings.UploadFilesDirectory);

            if (System.IO.File.Exists(fullPath))
            {
                var albums = SongFileParser.GetAlbumsPlayedFromFile(fullPath);

                if (albums == null || !albums.Any())
                {
                    resp.ErrorCode = (int)SongsFilesResponseCode.NoSongsInFile;
                    resp.FailReason = "No songs in file.";
                }
                else
                {
                    resp.AlbumsPlayed = new DisplayAlbum[albums.Count];
                    for(int i = 0; i < albums.Count; i++)
                        resp.AlbumsPlayed[i] = new DisplayAlbum
                        {
                            Date = albums[i].Date,
                            Location = albums[i].Location,
                            Artist = albums[i].Artist,
                            Album = albums[i].Album,
                            ImagePath = albums[i].ImagePath,
                            PlayerLink = albums[i].PlayerLink,
                            UserName = albums[i].UserName,
                            Id = albums[i].Id.ToString()
                        };
                }
            }
            else
            {
                resp.ErrorCode = (int)SongsFilesResponseCode.InvalidFile;
                resp.FailReason = "No such file exists.";
            }

            return resp;
        }

        [HttpGet("[action]/{fileKey}/{userId}")]
        [ProducesResponseType(201, Type = typeof(SongsFilesDetailsResponse))]
        [ProducesResponseType(400)]
        public SongsFilesDetailsResponse GetAllAlbumsPlayedFromFileToUser(string fileKey, string userId)
        {
            var resp = GetAllAlbumsPlayedFromFile(fileKey);

            if (resp.ErrorCode == (int) SongsFilesResponseCode.Success)
            {
                User foundUser = _userDatabase.LoadedItems.FirstOrDefault(x => x.Id.ToString() == userId);

                if (foundUser == null)
                {
                    resp.ErrorCode = (int)SongsFilesResponseCode.InvalidUser;
                    resp.FailReason = "No user for this id.";
                }
                else
                {
                    // Remove any items for this user from the table
                    RemoveExistingItemsForUser(foundUser.Name);

                    // Add the albums read from file with this user name
                    lock (Lock)
                    {
                        foreach (var fileAlbum in resp.AlbumsPlayed)
                        {
                            _albumPlayedDatabase.AddNewItemToDatabase(
                                new AlbumPlayed
                                {
                                    Date = fileAlbum.Date,
                                    Location = fileAlbum.Location,
                                    Artist = fileAlbum.Artist,
                                    Album = fileAlbum.Album,
                                    ImagePath = fileAlbum.ImagePath,
                                    PlayerLink = fileAlbum.PlayerLink,

                                    UserName = foundUser.Name
                                }
                                );
                        }
                    }

                }
            }
            
            return resp;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(201, Type = typeof(SongsValuesDetails))]
        public SongsValuesDetails GetSongsValuesDetails()
        {
            Dictionary<string, string> locationValues = new Dictionary<string, string>();
            Dictionary<string, string> artistValues = new Dictionary<string, string>();
            Dictionary<string, string> albumValues = new Dictionary<string, string>();

            lock (Lock)
            {
                foreach (var albumPlayed in _albumPlayedDatabase.LoadedItems)
                {
                    AddIfUnique(locationValues, albumPlayed.Location);
                    AddIfUnique(artistValues, albumPlayed.Artist);
                    AddIfUnique(albumValues, albumPlayed.Album);
                }
            }


            SongsValuesDetails details = new SongsValuesDetails
            {
                LocationValues = locationValues.Keys.ToArray(),
                ArtistValues = artistValues.Keys.ToArray(),
                AlbumValues = albumValues.Keys.ToArray()
            };

            return details;
        }

        /// <summary>
        /// Adds a new user login.
        /// </summary>
        /// <param name="addAlbumRequest">The new album played item to try to add.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] DisplayAlbum addAlbumRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AlbumPlayedAddResponse response = new AlbumPlayedAddResponse
            {
                ErrorCode = (int)UserResponseCode.Success,
                Album = new DisplayAlbum(addAlbumRequest),
                FailReason = "",
                UserId = ""
            };

            lock (Lock)
            {
                var newAlbum = new AlbumPlayed()
                {
                    Album = addAlbumRequest.Album,
                    Artist = addAlbumRequest.Artist,
                    Date = addAlbumRequest.Date,
                    ImagePath = addAlbumRequest.ImagePath,
                    Location = addAlbumRequest.Location,
                    PlayerLink = addAlbumRequest.PlayerLink,
                    UserName = addAlbumRequest.UserName
                };
                _albumPlayedDatabase.AddNewItemToDatabase(newAlbum);
            }

            return Ok(response);
        }


        // PUT api/values/5
        [HttpPut]
        public IActionResult UpdateAlbum([FromBody] DisplayAlbum existingAlbum)
        {
            // set up the successful response
            AlbumPlayedAddResponse response = new AlbumPlayedAddResponse
            {
                ErrorCode = (int)UserResponseCode.Success,
                Album = new DisplayAlbum(existingAlbum),
                FailReason = "",
                UserId = ""
            };

            // Find the item
            lock (Lock)
            {
                var itemToUpdate =
                    _albumPlayedDatabase.LoadedItems.FirstOrDefault(x => x.Id.ToString() == existingAlbum.Id);

                if (itemToUpdate == null)
                {
                    response.ErrorCode = (int)UserResponseCode.UnknownItem;
                    response.FailReason = "Could not find item";
                }
                else
                {
                    itemToUpdate.Date = existingAlbum.Date;
                    itemToUpdate.Location = existingAlbum.Location;
                    itemToUpdate.Artist = existingAlbum.Artist;
                    itemToUpdate.Album = existingAlbum.Album;
                    itemToUpdate.ImagePath = existingAlbum.ImagePath;
                    itemToUpdate.PlayerLink = existingAlbum.PlayerLink;

                    _albumPlayedDatabase.UpdateDatabaseItem(itemToUpdate);
                }
            }

            return Ok(response);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            // set up the successful response
            AlbumPlayedAddResponse response = new AlbumPlayedAddResponse
            {
                ErrorCode = (int)UserResponseCode.Success,
                Album = new DisplayAlbum(),
                FailReason = "",
                UserId = ""
            };

            // Find the item
            lock (Lock)
            {
                var itemToDelete =
                    _albumPlayedDatabase.LoadedItems.FirstOrDefault(x => x.Id.ToString() == id);

                if (itemToDelete == null)
                {
                    response.ErrorCode = (int)UserResponseCode.UnknownItem;
                    response.FailReason = "Could not find item";
                }
                else
                {

                    response.Album.Date = itemToDelete.Date;
                    response.Album.Location = itemToDelete.Location;
                    response.Album.Artist = itemToDelete.Artist;
                    response.Album.Album = itemToDelete.Album;
                    response.Album.ImagePath = itemToDelete.ImagePath;
                    response.Album.PlayerLink = itemToDelete.PlayerLink;

                    _albumPlayedDatabase.RemoveItemFromDatabase(itemToDelete);
                }
            }

            return Ok(response);
        }

        #endregion

        #region Constructor

        public DataModelController(
            IOptions<FileUploadSettings> uploaderConfig,
            IOptions<MongoDbSettings> dbConfig)
        {
            _dbConfig = dbConfig.Value;
            _uploaderSettings = uploaderConfig.Value;

            _albumPlayedDatabase = new AlbumPlayedDatabase(_dbConfig.ConnectionString);
            _userDatabase = new UserDatabase(_dbConfig.ConnectionString);
        }

        #endregion
    }
}
