

using System;
using System.Collections.ObjectModel;
using SongsDatabase.DataModels;

namespace SongsBurger.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using SongsBurger.Controllers.Settings;
    using SongsDatabase.Databases;


    [Route("api/[controller]")]
    [ApiController]
    public class DataModelController : ControllerBase
    {
        #region Private Data

        private readonly MongoDbSettings _dbConfig;
        private readonly UserDatabase _userDatabase;
        private readonly AlbumPlayedDatabase _albumPlayedDatabase;

        #endregion

        #region Initialisation

        private static readonly object Lock = new object();

        #endregion

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

        #region Constructor

        public DataModelController(IOptions<MongoDbSettings> dbConfig)
        {
            _dbConfig = dbConfig.Value;
            _userDatabase = new UserDatabase(_dbConfig.ConnectionString);
            _albumPlayedDatabase = new AlbumPlayedDatabase(_dbConfig.ConnectionString);
        }

        #endregion
    }
}
