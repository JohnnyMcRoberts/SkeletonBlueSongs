namespace SongsBurger.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using SongsDatabase.Databases;
    using SongsDatabase.DataModels;

    using RequestsResponses;
    using Settings;

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

        #region HTTP Handlers

        /// <summary>
        /// Adds a new user login.
        /// </summary>
        /// <param name="addRequest">The check new user login to try to add.</param>
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
                ErrorCode = (int)UserResponseCode.Success,
                Name = addRequest.Name,
                FailReason = "",
                UserId =""
            };

            if(_userDatabase.LoadedItems.Any(x => x.Name == addRequest.Name))
            {
                response.ErrorCode = (int)UserResponseCode.DuplicateName;
                response.FailReason = "Cannot Add as User name already exists";
                return Ok(response);
            }

            if (_userDatabase.LoadedItems.Any(x => x.Email == addRequest.Email))
            {
                response.ErrorCode = (int)UserResponseCode.DuplicateEmail;
                response.FailReason = "Cannot Add as User e-mail already exists";
                return Ok(response);
            }

            User newUser = new User(addRequest.Name, addRequest.Password, addRequest.Email, addRequest.Description);
            _userDatabase.AddNewItemToDatabase(newUser);

            response.UserId = newUser.Id.ToString();
            return Ok(response);
        }

        #endregion

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
