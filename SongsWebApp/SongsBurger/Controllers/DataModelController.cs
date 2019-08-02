using SongsBurger.Controllers.Settings;

namespace SongsBurger.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;


    [Route("api/[controller]")]
    [ApiController]
    public class DataModelController : ControllerBase
    {
        #region Private Data

        private readonly MongoDbSettings _dbConfig;

        #endregion

        #region Initialisation

        private static readonly object Lock = new object();
        private static bool _dbSetUp;

        //public static void InitializeDatabase(IDbSettings dbSettings)
        //{
        //    lock (Lock)
        //    {
        //        //Check if it has already run
        //        if (_dbSetUp)
        //        {
        //            return;
        //        }

        //        // Always ensure the database collections exist.
        //        DatabaseSetup databaseSetup = new DatabaseSetup();
        //        databaseSetup.SetupDatabaseCollections(dbSettings);

        //        //Mark as run
        //        _dbSetUp = true;
        //    }
        //}

        #endregion


        #region Constructor

        public DataModelController(IOptions<MongoDbSettings> dbConfig)
        {
            _dbConfig = dbConfig.Value;
        }

        #endregion
    }
}
