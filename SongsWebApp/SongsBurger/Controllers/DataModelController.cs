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

        #endregion

        #region HTTP Requests

        [HttpGet("[action]")]
        public IEnumerable<AlbumPlayed> GetAllAlbumsPlayed()
        {
            var albums = new ObservableCollection<AlbumPlayed>();

            lock (Lock)
            {
                foreach (var albumPlayed in _albumPlayedDatabase.LoadedItems)
                {
                    albums.Add(new AlbumPlayed(albumPlayed));
                }
            }

            if (albums.Count == 0)
            {
                albums.Add(
                    new AlbumPlayed
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
                    resp.AlbumsPlayed = albums.ToArray();
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
                        foreach (AlbumPlayed fileAlbum in resp.AlbumsPlayed)
                        {
                            _albumPlayedDatabase.AddNewItemToDatabase(
                                new AlbumPlayed(fileAlbum) { UserName = foundUser.Name }
                                );
                        }
                    }

                }
            }
            
            return resp;
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
