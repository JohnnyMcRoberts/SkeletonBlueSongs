// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserDatabase.cs" company="N/A">
//   2017-2086
// </copyright>
// <summary>
//   The album played MongoDb database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SongsDatabase.Databases
{
    using System;
    using System.Collections.ObjectModel;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using DataModels;

    /// <summary>
    /// The albums played database.
    /// </summary>
    public class AlbumPlayedDatabase : BaseDatabaseConnection<AlbumPlayed>
    {
        /// <summary>
        /// Gets or sets the connection string for the database.
        /// </summary>
        public sealed override string DatabaseConnectionString { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the collection.
        /// </summary>
        public sealed override string CollectionName { get; protected set; }

        /// <summary>
        /// Gets or sets the database collection filter.
        /// </summary>
        public sealed override FilterDefinition<AlbumPlayed> Filter { get; protected set; }

        /// <summary>
        /// Gets or sets the set of items from the database.
        /// </summary>
        public sealed override ObservableCollection<AlbumPlayed> LoadedItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumPlayedDatabase"/> class.
        /// </summary>
        /// <param name="dbConnection">The connection string for the database.</param>
        public AlbumPlayedDatabase(string dbConnection)
        {
            DatabaseConnectionString = dbConnection;

            LoadedItems = new ObservableCollection<AlbumPlayed>();

            // This is a query to get everything to date. 
            Filter = Builders<AlbumPlayed>.Filter.Lte(
                new StringFieldDefinition<AlbumPlayed, BsonDateTime>("date"), new BsonDateTime(DateTime.Now.AddDays(1000)));

            CollectionName = "albums_played";
            ConnectToDatabase();
        }
    }
}