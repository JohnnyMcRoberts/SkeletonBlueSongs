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
    public class UserDatabaseTest
    {
        #region Constants

        private readonly Dictionary<string, string> _settingsLookup;
        private const string ConnectionKey = "MongoDbConnectionString";
        private const string UpdatedUserName = "Updated test name";

        #endregion

        #region Utility Functions

        private static void GetTestUser(out string name, out User user)
        {
            name = Guid.NewGuid().ToString();
            string password = "testPass101";
            string email = "test@usertest.com";
            string description = "a test description";
            user = new User(name, password, email, description);
        }

        #endregion

        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsTrue(_settingsLookup.ContainsKey(ConnectionKey));

            UserDatabase userDb = new UserDatabase(_settingsLookup[ConnectionKey]);

            Assert.IsNotNull(userDb.LoadedItems);
        }

        [TestMethod]
        public void TestAddRemove()
        {
            UserDatabase userDb = new UserDatabase(_settingsLookup[ConnectionKey]);
            GetTestUser(out var name, out User user);

            // Test User is not there
            Assert.IsNotNull(userDb.LoadedItems);
            int numberInitialItems = userDb.LoadedItems.Count;
            List<string> userNames = userDb.LoadedItems.ToList().Select(x => x.Name).ToList();
            Assert.IsFalse(userNames.Contains(name));

            // Add new User
            userDb.AddNewItemToDatabase(user);
            userDb.ConnectToDatabase();
            Assert.AreNotEqual(numberInitialItems, userDb.LoadedItems.Count);
            Assert.AreEqual(1, userDb.LoadedItems.Count - numberInitialItems);
            List<string> userNames2 = userDb.LoadedItems.ToList().Select(x => x.Name).ToList();
            Assert.IsTrue(userNames2.Contains(name));

            // Remove the new user
            userDb.RemoveItemFromDatabase(user);
            userDb.ConnectToDatabase();

            Assert.AreEqual(userDb.LoadedItems.Count, numberInitialItems);
            List<string> userNames3 = userDb.LoadedItems.ToList().Select(x => x.Name).ToList();
            Assert.IsFalse(userNames3.Contains(name));
        }

        [TestMethod]
        public void TestAddUpdateRemove()
        {
            UserDatabase userDb = new UserDatabase(_settingsLookup[ConnectionKey]);
            GetTestUser(out var name, out User user);

            // Test User is not there
            Assert.IsNotNull(userDb.LoadedItems);
            int numberInitialItems = userDb.LoadedItems.Count;
            List<string> userNames = userDb.LoadedItems.ToList().Select(x => x.Name).ToList();
            Assert.IsFalse(userNames.Contains(name));

            // Add new User
            userDb.AddNewItemToDatabase(user);
            userDb.ConnectToDatabase();
            Assert.AreNotEqual(numberInitialItems, userDb.LoadedItems.Count);
            Assert.AreEqual(1, userDb.LoadedItems.Count - numberInitialItems);
            List<string> userNames2 = userDb.LoadedItems.ToList().Select(x => x.Name).ToList();
            Assert.IsTrue(userNames2.Contains(name));

            // Update the User to have different name
            user.Name = UpdatedUserName;
            userDb.UpdateDatabaseItem(user);
            userDb.ConnectToDatabase();
            List<string> userNames3 = userDb.LoadedItems.ToList().Select(x => x.Name).ToList();
            Assert.IsTrue(userNames3.Contains(UpdatedUserName));

            // Remove the new user
            userDb.RemoveItemFromDatabase(user);
            userDb.ConnectToDatabase();

            Assert.AreEqual(userDb.LoadedItems.Count, numberInitialItems);
            List<string> userNames4 = userDb.LoadedItems.ToList().Select(x => x.Name).ToList();
            Assert.IsFalse(userNames4.Contains(name));
        }

        #region Constants

        public UserDatabaseTest()
        {
            _settingsLookup = TestUtilities.GetSettingsLookup();
        }

        #endregion
    }
}
