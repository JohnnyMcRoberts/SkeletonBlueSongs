using SongsDatabase.DataModels;

namespace SongsDatabase.Databases
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using MongoDB.Driver;

    using Base;
    using Interfaces;

    public abstract class BaseDatabaseConnection<T> : IDatabaseConnection<T> where T : BaseMongoEntity
    {
        #region Constants

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        public string DatabaseName => "songs_played";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the connection string for the database.
        /// </summary>
        public abstract string DatabaseConnectionString { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the collection.
        /// </summary>
        public abstract string CollectionName { get; protected set; }

        /// <summary>
        /// Gets or sets the database collection filter.
        /// </summary>
        public abstract FilterDefinition<T> Filter { get; protected set; }

        /// <summary>
        /// Gets or sets the set of items from the database.
        /// </summary>
        public abstract ObservableCollection<T> LoadedItems { get; set; }

        /// <summary>
        /// Gets or sets the connection to the database.
        /// </summary>
        public IMongoClient Client { get; protected set; }

        /// <summary>
        /// Gets or sets the items database.
        /// </summary>
        public IMongoDatabase ItemsDatabase { get; protected set; }

        /// <summary>
        /// Gets or sets whether read items database.
        /// </summary>
        public bool ReadFromDatabase { get; protected set; }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Updates an existing items in the database.
        /// </summary>
        /// <param name="itemsRead">The items retrieved from the database.</param>
        private void UpdateDatabaseItems(IMongoCollection<T> itemsRead)
        {
            List<T> missingItems = new List<T>();
            List<T> existingItems;

            using (var cursor = itemsRead.FindSync(Filter))
            {
                existingItems = cursor.ToList();
            }

            // Get the missing items.
            foreach (var loaded in LoadedItems)
            {
                bool alreadyThere = false;
                foreach (var existing in existingItems)
                {
                    if (ItemsEquivalent(loaded, existing))
                    {
                        alreadyThere = true;
                        break;
                    }
                }

                if (!alreadyThere)
                    missingItems.Add(loaded);
            }

            // Then insert them to the list.
            itemsRead.InsertMany(missingItems);
            itemsRead.Count(Filter);
        }

        /// <summary>
        /// Loads all the items from the database into the list.
        /// </summary>
        /// <param name="itemsRead">The items retrieved from the database.</param>
        private void LoadAllItemsFromDatabase(IMongoCollection<T> itemsRead)
        {
            LoadedItems.Clear();

            using (var cursor = itemsRead.FindSync(Filter))
            {
                var countryList = cursor.ToList();
                foreach (var country in countryList)
                {
                    LoadedItems.Add(country);
                }
            }
        }

        /// <summary>
        /// Adds all the new items into the database.
        /// </summary>
        /// <param name="itemsInDatabase">The items retrieved from the database.</param>
        private void AddNewItemsToExistingDatabase(IMongoCollection<T> itemsInDatabase)
        {
            ObservableCollection<T> dbItems = new ObservableCollection<T>();
            ObservableCollection<T> missingItems = new ObservableCollection<T>();

            using (IAsyncCursor<T> cursor = itemsInDatabase.FindSync(Filter))
            {
                List<T> itemList = cursor.ToList();
                foreach (var item in itemList)
                {
                    dbItems.Add(item);
                }
            }

            foreach (var currentItem in LoadedItems)
            {
                bool itemInDb = false;
                foreach (var dbItem in dbItems)
                {
                    if (ItemsEquivalent(dbItem, currentItem))
                        itemInDb = true;
                    if (itemInDb)
                        break;
                }

                if (!itemInDb)
                    missingItems.Add(currentItem);
            }

            if (missingItems.Any())
            {
                itemsInDatabase.InsertMany(missingItems);
                itemsInDatabase.Count(Filter);
            }
        }

        /// <summary>
        /// Adds all the new items into the empty database.
        /// </summary>
        /// <param name="itemsInDatabase">The items retrieved from the database.</param>
        /// <returns>The number of items in the database</returns>
        private long AddLoadedItemsToBlankDatabase(IMongoCollection<T> itemsInDatabase)
        {
            itemsInDatabase.InsertMany(LoadedItems);
            return itemsInDatabase.Count(Filter);
        }

        private bool CheckRemoved(bool removed)
        {
            var collection = Client.GetDatabase(DatabaseName).GetCollection<T>(CollectionName);

            List<T> reloadedItems = new List<T>();
            using (var cursor = collection.FindSync(Filter))
            {
                var countryList = cursor.ToList();
                foreach (var country in countryList)
                {
                    reloadedItems.Add(country);
                }
            }

            if (reloadedItems.Count != LoadedItems.Count)
            {
                removed = true;
                LoadedItems.Clear();
                foreach (var item in reloadedItems)
                    LoadedItems.Add(item);
            }

            return removed;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets if two items are equivalent.
        /// </summary>
        /// <param name="itemA">The first item.</param>
        /// <param name="itemB">The second item.</param>
        /// <returns>True if equivalent, false otherwise</returns>
        public virtual bool ItemsEquivalent(T itemA, T itemB)
        {
            return itemA.EquivalenceName == itemB.EquivalenceName;
        }

        /// <summary>
        /// Adds a new item to the database.
        /// </summary>
        /// <param name="newItem">The item to add.</param>
        public void AddNewItemToDatabase(T newItem)
        {
            Client = new MongoClient(DatabaseConnectionString);

            ItemsDatabase = Client.GetDatabase(DatabaseName);

            IMongoCollection<T> itemsRead = ItemsDatabase.GetCollection<T>(CollectionName);

            itemsRead.InsertOne(newItem);
        }

        /// <summary>
        /// Removes an item from the database.
        /// </summary>
        /// <param name="deleteItem">The item to remove.</param>
        public bool RemoveItemFromDatabase(T deleteItem)
        {
            bool removed = false;
            Client = new MongoClient(DatabaseConnectionString);

            ItemsDatabase = Client.GetDatabase(DatabaseName);

            IMongoCollection<T> itemsRead = ItemsDatabase.GetCollection<T>(CollectionName);
            LoadAllItemsFromDatabase(itemsRead);
            T foundItem = LoadedItems .Where(x => x.EquivalenceName == deleteItem.EquivalenceName) .Select(x => x) .FirstOrDefault();

            if (foundItem != null)
            {
                itemsRead.DeleteOne(a => a.Id == foundItem.Id);

                removed = CheckRemoved(removed);
            }

            return removed;
        }

        /// <summary>
        /// Updates an existing item in the database.
        /// </summary>
        /// <param name="existingItem">The item to update.</param>
        public void UpdateDatabaseItem(T existingItem)
        {
            Client = new MongoClient(DatabaseConnectionString);

            ItemsDatabase = Client.GetDatabase(DatabaseName);

            IMongoCollection<T> itemsRead = ItemsDatabase.GetCollection<T>(CollectionName);

            var filterOnId = Builders<T>.Filter.Eq(s => s.Id, existingItem.Id);

            long totalCount = itemsRead.Count(filterOnId);

            var result = itemsRead.ReplaceOne(filterOnId, existingItem);
        }

        /// <summary>
        /// Connects to the database and sets up the loaded items from the collection or vice versa.
        /// </summary>
        public void ConnectToDatabase()
        {
            ReadFromDatabase = false;
            Client = new MongoClient(DatabaseConnectionString);
            ItemsDatabase = Client.GetDatabase(DatabaseName);
            IMongoCollection<T> itemsRead = ItemsDatabase.GetCollection<T>(CollectionName);

            long totalCount = itemsRead.Count(Filter);

            if (totalCount == 0 && LoadedItems.Count != 0)
            {
                AddLoadedItemsToBlankDatabase(itemsRead);
            }
            else if (LoadedItems.Count != 0 && LoadedItems.Count > totalCount)
            {
                AddNewItemsToExistingDatabase(itemsRead);
            }
            else if (totalCount != 0 && LoadedItems.Count <= totalCount)
            {
                LoadAllItemsFromDatabase(itemsRead);
                ReadFromDatabase = true;
            }
            else if (totalCount != 0 && totalCount < LoadedItems.Count)
            {
                UpdateDatabaseItems(itemsRead);
                ReadFromDatabase = true;
            }
        }

        #endregion
    }
}
