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
    public class SongFileParserTest
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

            // Check the location, artist & album
            Assert.AreEqual("To Work", album.Location);
            Assert.AreEqual("Current 93", album.Artist);
            Assert.AreEqual("Halo", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
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
            Assert.AreEqual(string.Empty, album.UserName);
        }

        public static void CheckAlbum_2(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(23, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("In Work", album.Location);
            Assert.AreEqual("Lubomyr Melnyk", album.Artist);
            Assert.AreEqual("Rivers And Streams", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
        }


        public static void CheckAlbum_3(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(24, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("To Work", album.Location);
            Assert.AreEqual("Current 93", album.Artist);
            Assert.AreEqual("Halo", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
        }

        public static void CheckAlbum_4(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(24, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("In Work", album.Location);
            Assert.AreEqual("Nick Cave & the Bad Seeds", album.Artist);
            Assert.AreEqual("Live from KCRW", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
        }

        public static void CheckAlbum_5(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(24, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("In Work", album.Location);
            Assert.AreEqual("Acid Mothers Temple & The Melting Paraiso U.F.O.", album.Artist);
            Assert.AreEqual("IAO Chant From the Melting Paraiso Underground Freak Out", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
        }


        public static void CheckAlbum_6(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(25, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("To Work", album.Location);
            Assert.AreEqual("The Fall", album.Artist);
            Assert.AreEqual("Palace of Swords Reversed", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
        }

        public static void CheckAlbum_7(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(25, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("In Work", album.Location);
            Assert.AreEqual("The Fall", album.Artist);
            Assert.AreEqual("Extricate", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
        }

        public static void CheckAlbum_8(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2018, album.Date.Year);
            Assert.AreEqual(1, album.Date.Month);
            Assert.AreEqual(25, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("In Work", album.Location);
            Assert.AreEqual("The Fall", album.Artist);
            Assert.AreEqual("Sub-Lingual Tablet", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
        }


        public static void CheckAlbum_Last(AlbumPlayed album)
        {
            Assert.IsNotNull(album);

            // Check the date
            Assert.AreEqual(2019, album.Date.Year);
            Assert.AreEqual(8, album.Date.Month);
            Assert.AreEqual(7, album.Date.Day);

            // Check the location, artist & album
            Assert.AreEqual("In Work", album.Location);
            Assert.AreEqual("Trash Kit", album.Artist);
            Assert.AreEqual("Confidence", album.Album);

            // check the other items are still blank
            Assert.AreEqual(string.Empty, album.ImagePath);
            Assert.AreEqual(string.Empty, album.PlayerLink);
            Assert.AreEqual(string.Empty, album.UserName);
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

            var albums = SongFileParser.GetAlbumsPlayedFromFile(invalidFile);
            Assert.IsNotNull(albums);
            Assert.AreEqual(0, albums.Count);
        }

        [TestMethod]
        public void TestValidFile_SingleAlbumDay()
        {
            // Check the file is OK
            string singleAlbumPath = PathUtilities.GetPathToFile(PathUtilities.TestFileNameSingleAlbum);
            Assert.IsTrue(File.Exists(singleAlbumPath));

            // Get the album
            List<AlbumPlayed> albums = SongFileParser.GetAlbumsPlayedFromFile(singleAlbumPath);
            Assert.IsNotNull(albums);
            Assert.AreEqual(1, albums.Count);

            // Check the first LP
            CheckAlbum_0(albums[0]);
        }

        [TestMethod]
        public void TestValidFile_TestFileNameSingleDay()
        {
            // Check the file is OK
            string singleDayPath = PathUtilities.GetPathToFile(PathUtilities.TestFileNameSingleDay);
            Assert.IsTrue(File.Exists(singleDayPath));

            // Get the albums
            List<AlbumPlayed> albums = SongFileParser.GetAlbumsPlayedFromFile(singleDayPath);
            Assert.IsNotNull(albums);
            Assert.AreEqual(2, albums.Count);

            // Check the first LP
            CheckAlbum_0(albums[0]);

            // Check the second LP
            CheckAlbum_1(albums[1]);
        }

        [TestMethod]
        public void TestValidFile_TestFileNameMultipleSongsSingleDay()
        {
            // Check the file is OK
            string singleDayPath = PathUtilities.GetPathToFile(PathUtilities.TestFileNameMultipleSongsSingleDay);
            Assert.IsTrue(File.Exists(singleDayPath));

            // Get the albums
            List<AlbumPlayed> albums = SongFileParser.GetAlbumsPlayedFromFile(singleDayPath);
            Assert.IsNotNull(albums);
            Assert.AreEqual(3, albums.Count);

            // Check the first LP
            CheckAlbum_0(albums[0]);

            // Check the second LP
            CheckAlbum_1(albums[1]);

            // Check the third LP
            CheckAlbum_2(albums[2]);
        }

        [TestMethod]
        public void TestValidFile_TestFileNameMultipleDaysSongs()
        {
            // Check the file is OK
            string singleDayPath = PathUtilities.GetPathToFile(PathUtilities.TestFileNameMultipleDaysSongs);
            Assert.IsTrue(File.Exists(singleDayPath));

            // Get the albums
            List<AlbumPlayed> albums = SongFileParser.GetAlbumsPlayedFromFile(singleDayPath);
            Assert.IsNotNull(albums);
            Assert.AreEqual(9, albums.Count);

            // Check the first Day's LP
            CheckAlbum_0(albums[0]);
            CheckAlbum_1(albums[1]);
            CheckAlbum_2(albums[2]);

            // Check the second Day's LP
            CheckAlbum_3(albums[3]);
            CheckAlbum_4(albums[4]);
            CheckAlbum_5(albums[5]);

            // Check the third Day's LP
            CheckAlbum_6(albums[6]);
            CheckAlbum_7(albums[7]);
            CheckAlbum_8(albums[8]);
        }

        [TestMethod]
        public void TestValidFile_TestFileNameAllDays()
        {
            // Check the file is OK
            string singleDayPath = PathUtilities.GetPathToFile(PathUtilities.TestFileNameAllDays);
            Assert.IsTrue(File.Exists(singleDayPath));

            // Get the albums
            List<AlbumPlayed> albums = SongFileParser.GetAlbumsPlayedFromFile(singleDayPath);
            Assert.IsNotNull(albums);
            Assert.IsTrue(albums.Count > 900);

            // Check the first Day's LP
            CheckAlbum_0(albums[0]);
            CheckAlbum_1(albums[1]);
            CheckAlbum_2(albums[2]);

            // Check the second Day's LP
            CheckAlbum_3(albums[3]);
            CheckAlbum_4(albums[4]);
            CheckAlbum_5(albums[5]);

            // Check the third Day's LP
            CheckAlbum_6(albums[6]);
            CheckAlbum_7(albums[7]);
            CheckAlbum_8(albums[8]);


            // Check the last LP
            CheckAlbum_Last(albums.Last());

        }

        #endregion

        #region Constructor

        public SongFileParserTest()
        {
            TestFilePathAllDays = PathUtilities.GetPathToFile(PathUtilities.TestFileNameAllDays);
        }

        #endregion
    }
}
