﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserDatabase.cs" company="N/A">
//   2017-2086
// </copyright>
// <summary>
//   The user MongoDb database.
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
    /// The users database.
    /// </summary>
    public class UserDatabase : BaseDatabaseConnection<User>
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
        public sealed override FilterDefinition<User> Filter { get; protected set; }

        /// <summary>
        /// Gets or sets the set of items from the database.
        /// </summary>
        public sealed override ObservableCollection<User> LoadedItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDatabase"/> class.
        /// </summary>
        /// <param name="dbConnection">The connection string for the database.</param>
        public UserDatabase(string dbConnection)
        {
            DatabaseConnectionString = dbConnection;

            LoadedItems = new ObservableCollection<User>();

            // This is a query to get everything to date. 
            Filter = Builders<User>.Filter.Lte(
                new StringFieldDefinition<User, BsonDateTime>("date_added"), new BsonDateTime(DateTime.Now.AddDays(1000)));

            CollectionName = "users";
            ConnectToDatabase();
        }
    }
}