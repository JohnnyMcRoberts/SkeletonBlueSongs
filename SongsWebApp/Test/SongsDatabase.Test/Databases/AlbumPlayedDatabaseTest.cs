namespace SongsDatabase.Test.Databases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SongsDatabase.Databases;

    using DataModels;
    using Utilities;

    [TestClass]
    public class AlbumPlayedDatabaseTest
    {
        #region Constants

        private readonly Dictionary<string, string> _settingsLookup;
        private const string ConnectionKey = "MongoDbConnectionString";
        private const string UpdatedAlbumName = "Updated album name";

        #endregion

        #region Utility Functions

        private static void GetTestAlbumPlayed(out string albumPlayedName, out AlbumPlayed albumPlayed)
        {
            albumPlayedName = Guid.NewGuid().ToString();
            string artist = "a test artist";
            string location = "To Work";
            string userName = "a test user";
            albumPlayed = new AlbumPlayed
            {
                Album = albumPlayedName,
                Artist = artist,
                Date = DateTime.Now,
                Location = location,
                UserName = userName
            };
        }

        #endregion

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsTrue(_settingsLookup.ContainsKey(ConnectionKey));

            AlbumPlayedDatabase albumPlayedDatabase = new AlbumPlayedDatabase(_settingsLookup[ConnectionKey]);

            Assert.IsNotNull(albumPlayedDatabase.LoadedItems);
        }

        [TestMethod]
        public void TestAddRemove()
        {
            AlbumPlayedDatabase albumPlayedDatabase = new AlbumPlayedDatabase(_settingsLookup[ConnectionKey]);
            GetTestAlbumPlayed(out var name, out AlbumPlayed albumPlayed);

            // Test User is not there
            Assert.IsNotNull(albumPlayedDatabase.LoadedItems);
            int numberInitialItems = albumPlayedDatabase.LoadedItems.Count;
            List<string> albumNames = albumPlayedDatabase.LoadedItems.ToList().Select(x => x.Album).ToList();
            Assert.IsFalse(albumNames.Contains(name));

            // Add new User
            albumPlayedDatabase.AddNewItemToDatabase(albumPlayed);
            albumPlayedDatabase.ConnectToDatabase();
            Assert.AreNotEqual(numberInitialItems, albumPlayedDatabase.LoadedItems.Count);
            Assert.AreEqual(1, albumPlayedDatabase.LoadedItems.Count - numberInitialItems);
            List<string> albumNames2 = albumPlayedDatabase.LoadedItems.ToList().Select(x => x.Album).ToList();
            Assert.IsTrue(albumNames2.Contains(name));

            // Remove the new albumPlayed
            albumPlayedDatabase.RemoveItemFromDatabase(albumPlayed);
            albumPlayedDatabase.ConnectToDatabase();

            Assert.AreEqual(albumPlayedDatabase.LoadedItems.Count, numberInitialItems);
            List<string> albumNames3 = albumPlayedDatabase.LoadedItems.ToList().Select(x => x.Album).ToList();
            Assert.IsFalse(albumNames3.Contains(name));
        }

        [TestMethod]
        public void TestAddUpdateRemove()
        {
            AlbumPlayedDatabase albumPlayedDatabase = new AlbumPlayedDatabase(_settingsLookup[ConnectionKey]);
            GetTestAlbumPlayed(out var name, out AlbumPlayed albumPlayed);

            // Test album played is not there
            Assert.IsNotNull(albumPlayedDatabase.LoadedItems);
            int numberInitialItems = albumPlayedDatabase.LoadedItems.Count;
            List<string> userNames = albumPlayedDatabase.LoadedItems.ToList().Select(x => x.Album).ToList();
            Assert.IsFalse(userNames.Contains(name));

            // Add new album played
            albumPlayedDatabase.AddNewItemToDatabase(albumPlayed);
            albumPlayedDatabase.ConnectToDatabase();
            Assert.AreNotEqual(numberInitialItems, albumPlayedDatabase.LoadedItems.Count);
            Assert.AreEqual(1, albumPlayedDatabase.LoadedItems.Count - numberInitialItems);
            List<string> userNames2 = albumPlayedDatabase.LoadedItems.ToList().Select(x => x.Album).ToList();
            Assert.IsTrue(userNames2.Contains(name));

            // Update the album played to have different album name
            albumPlayed.Album = UpdatedAlbumName;
            albumPlayedDatabase.UpdateDatabaseItem(albumPlayed);
            albumPlayedDatabase.ConnectToDatabase();
            List<string> userNames3 = albumPlayedDatabase.LoadedItems.ToList().Select(x => x.Album).ToList();
            Assert.IsTrue(userNames3.Contains(UpdatedAlbumName));

            // Remove the new album played
            albumPlayedDatabase.RemoveItemFromDatabase(albumPlayed);
            albumPlayedDatabase.ConnectToDatabase();

            Assert.AreEqual(albumPlayedDatabase.LoadedItems.Count, numberInitialItems);
            List<string> userNames4 = albumPlayedDatabase.LoadedItems.ToList().Select(x => x.Album).ToList();
            Assert.IsFalse(userNames4.Contains(UpdatedAlbumName));
        }

        #region Constants

        public AlbumPlayedDatabaseTest()
        {
            _settingsLookup = TestUtilities.GetSettingsLookup();
        }

        #endregion
    }
}
