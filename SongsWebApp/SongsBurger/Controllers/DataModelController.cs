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

        #endregion

        #region Initialisation

        private static readonly object Lock = new object();

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
            albums.Add( 
                new AlbumPlayed
                {
                    Artist = "a",
                    Album = "b",
                    Date = DateTime.Now,
                    Location = "c",
                    UserName = "d"
                });
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

        #endregion

        #region Constructor

        public DataModelController(
            IOptions<FileUploadSettings> uploaderConfig,
            IOptions<MongoDbSettings> dbConfig)
        {
            _dbConfig = dbConfig.Value;
            _uploaderSettings = uploaderConfig.Value;

            _albumPlayedDatabase = new AlbumPlayedDatabase(_dbConfig.ConnectionString);
        }

        #endregion
    }
}
