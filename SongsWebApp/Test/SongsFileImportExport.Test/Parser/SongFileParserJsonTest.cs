namespace SongsFileImportExport.Test.Parser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;


    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SongsDatabase.DataModels;

    using SongsFileImportExport.Parser;

    using TestUtilities;

    [TestClass]
    public class SongFileParserJsonTest
    {
        #region Constants

        public readonly string TestFilePathAllDays;

        #endregion

        #region Test utility Functions

        public static void CheckAlbum_0(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(23, album.Date.Day);

            // Check the location, artist & album etc
            Assert.AreEqual("To Work", album.Location);
            Assert.AreEqual("Current 93", album.Artist);
            Assert.AreEqual("Halo", album.Album);

            Assert.AreEqual("https://upload.wikimedia.org/wikipedia/en/f/ff/Halo_album_cover.jpg", album.ImagePath);
            Assert.AreEqual("https://www.youtube.com/watch?v=i_sBbgptcQE", album.PlayerLink);
            Assert.AreEqual("JMcR", album.UserName);
        }

        public static void CheckAlbum_1(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(23, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("In Work", album.Location);
            Assert.AreEqual("Alice Coltrane", album.Artist);
            Assert.AreEqual("World Spirituality Classics 1: The Ecstatic Music of Alice Coltrane Turiyasangitananda", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual("JMcR", album.UserName);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetPathToFile()
        {
            Assert.IsTrue(File.Exists(TestFilePathAllDays));
        }

        [TestMethod]
        public void TestInvalidFile()
        {
            Assert.IsTrue(File.Exists(TestFilePathAllDays));

            string invalidFile = PathUtilities.TestFileNameAllDays + ".nothing";

            Assert.IsFalse(File.Exists(invalidFile));

            var albums = SongFileParserJson.GetAlbumsPlayedFromFile(invalidFile);
            Assert.IsNotNull(albums);
            Assert.AreEqual(0, albums.Count);
        }

        [TestMethod]
        public void TestValidFile_TestFileNameSingleDay()
        {
            // Check the file is OK
            string singleDayPath = PathUtilities.GetPathToFile(PathUtilities.TestFileNameSingleDayJson);
            Assert.IsTrue(File.Exists(singleDayPath));

            // Get the albums
            List<AlbumPlayed> albums = SongFileParserJson.GetAlbumsPlayedFromFile(singleDayPath);
            Assert.IsNotNull(albums);
            Assert.AreEqual(2, albums.Count);

            // Check the first LP
            CheckAlbum_0(albums[0]);

            // Check the second LP
            CheckAlbum_1(albums[1]);
        }

        [TestMethod]
        public void TestValidFile_TestFileNameAllDaysJson()
        {
            // Check the file is OK
            string singleDayPath = PathUtilities.GetPathToFile(PathUtilities.TestFileNameAllDaysJson);
            Assert.IsTrue(File.Exists(singleDayPath));

            // Get the albums
            List<AlbumPlayed> albums = SongFileParserJson.GetAlbumsPlayedFromFile(singleDayPath);
            Assert.IsNotNull(albums);
            Assert.AreEqual(923, albums.Count);

            // Check the first LP
            CheckAlbum_0(albums[0]);

            // Check the second LP
            CheckAlbum_1(albums[1]);
        }

        #endregion

        #region Constructor

        public SongFileParserJsonTest()
        {
            TestFilePathAllDays = PathUtilities.GetPathToFile(PathUtilities.TestFileNameAllDaysJson);
        }

        #endregion
    }
}
