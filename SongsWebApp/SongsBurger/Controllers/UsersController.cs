

using System;
using System.Collections.ObjectModel;
using SongsBurger.Controllers.RequestsResponses;
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
    public class UsersController : ControllerBase
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

        /// <summary>
        /// Finds the most similar items for the .
        /// </summary>
        /// <param name="addRequest">The check new quote part request.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] UserAddRequest addRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserAddResponse response = new UserAddResponse
            {
                ErrorCode = (int)UserAddResponseCode.Success,
                Name = addRequest.Name,
                FailReason = "",
                UserId =""
            };

            if(_userDatabase.LoadedItems.Any(x => x.Name == addRequest.Name))
            {
                response.ErrorCode = (int)UserAddResponseCode.DuplicateName;
                response.FailReason = "Cannot Add as User name already exists";
                return Ok(response);
            }

            if (_userDatabase.LoadedItems.Any(x => x.Email == addRequest.Email))
            {
                response.ErrorCode = (int)UserAddResponseCode.DuplicateEmail;
                response.FailReason = "Cannot Add as User e-mail already exists";
                return Ok(response);
            }

            User newUser = new User(addRequest.Name, addRequest.Password, addRequest.Email, addRequest.Description);
            _userDatabase.AddNewItemToDatabase(newUser);

            response.UserId = newUser.Id.ToString();
            return Ok(response);
        }


        #region Constructor

            public UsersController(IOptions<MongoDbSettings> dbConfig)
        {
            _dbConfig = dbConfig.Value;
            _userDatabase = new UserDatabase(_dbConfig.ConnectionString);
            _albumPlayedDatabase = new AlbumPlayedDatabase(_dbConfig.ConnectionString);
        }

        #endregion
    }
}
