using System;
using System.Collections.Generic;
using SongsDatabase.DataModels;

namespace SongsFileImportExport.Test.Exporter
{
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SongsFileImportExport.Exporter;

    using TestUtilities;

    [TestClass]
    public class AlbumsPlayedExporterTest
    {
        #region Constants

        public const string SingleAlbumOnlyOutput =
            "\nTuesday 23rd January 2018\n\n\tTo Work\n\n\t\tCurrent 93\n\t\t\tHalo\n";

        public const string TwoAlbumsSingleDayOutput =
            "\nTuesday 23rd January 2018\n\n\tTo Work\n\n\t\tCurrent 93\n\t\t\tHalo\n\n\tIn Work\n\n\t\tAlice Coltrane\n\t\t\tWorld Spirituality Classics 1: The Ecstatic Music of Alice Coltrane Turiyasangitananda\n";

        public const string ThreeAlbumsSingleDayOutput =
            "\nTuesday 23rd January 2018\n\n\tTo Work\n\n\t\tCurrent 93\n\t\t\tHalo\n\n\tIn Work\n\n\t\tAlice Coltrane\n\t\t\tWorld Spirituality Classics 1: The Ecstatic Music of Alice Coltrane Turiyasangitananda\n\n\t\tLubomyr Melnyk\n\t\t\tRivers And Streams\n";

        public const string FiveAlbumsTwoDaysOutput =
            "\nTuesday 23rd January 2018\n\n\tTo Work\n\n\t\tCurrent 93\n\t\t\tHalo\n\n\tIn Work\n\n\t\tAlice Coltrane\n\t\t\tWorld Spirituality Classics 1: The Ecstatic Music of Alice Coltrane Turiyasangitananda\n\n\t\tLubomyr Melnyk\n\t\t\tRivers And Streams\n\nThursday 25th January 2018\n\n\tIn Work\n\n\t\tThe Fall\n\t\t\tExtricate\n\t\t\tSub-Lingual Tablet\n";


        public readonly string TestFilePathAllDays;

        public readonly AlbumPlayed Album_0 = new AlbumPlayed
        {
            Date = new DateTime(2018,1, 23, 12, 0, 0),
            Location = "To Work",
            Artist = "Current 93",
            Album = "Halo"
        };

        public readonly AlbumPlayed Album_1 = new AlbumPlayed
        {
            Date = new DateTime(2018, 1, 23, 12, 1, 0),
            Location = "In Work",
            Artist = "Alice Coltrane",
            Album = "World Spirituality Classics 1: The Ecstatic Music of Alice Coltrane Turiyasangitananda"
        };

        public readonly AlbumPlayed Album_2 = new AlbumPlayed()
        {
            Date = new DateTime(2018, 1, 23, 12, 2, 0),
            Location = "In Work",
            Artist = "Lubomyr Melnyk",
            Album = "Rivers And Streams"
        };


        public readonly AlbumPlayed Album_3 = new AlbumPlayed()
        {
            Date = new DateTime(2018, 1, 25, 12, 2, 0),
            Location = "In Work",
            Artist = "The Fall",
            Album = "Extricate"
        };


        public readonly AlbumPlayed Album_4 = new AlbumPlayed()
        {
            Date = new DateTime(2018, 1, 25, 12, 2, 0),
            Location = "In Work",
            Artist = "The Fall",
            Album = "Sub-Lingual Tablet"
        };

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetPathToFile()
        {
            Assert.IsTrue(File.Exists(TestFilePathAllDays));
        }


        [TestMethod]
        public void TestEmptyArray()
        {
            AlbumsPlayedExporter.ExportToFile(new List<AlbumPlayed>(), out var fileContent);

            Assert.IsNotNull(fileContent);
            Assert.AreEqual(string.Empty, fileContent);
        }


        [TestMethod]
        public void TestSingleAlbum()
        {
            AlbumsPlayedExporter.ExportToFile(new List<AlbumPlayed> { Album_0 }, out var fileContent);

            Assert.IsNotNull(fileContent);
            Assert.AreEqual(SingleAlbumOnlyOutput, fileContent);
        }

        [TestMethod]
        public void TestTwoAlbumsSingleDay()
        {
            AlbumsPlayedExporter.ExportToFile(new List<AlbumPlayed> { Album_0, Album_1 }, out var fileContent);

            Assert.IsNotNull(fileContent);
            Assert.AreEqual(TwoAlbumsSingleDayOutput, fileContent);
        }

        [TestMethod]
        public void TestThreeAlbumsSingleDay()
        {
            AlbumsPlayedExporter.ExportToFile(new List<AlbumPlayed> { Album_0, Album_1, Album_2 }, out var fileContent);

            Assert.IsNotNull(fileContent);
            Assert.AreEqual(ThreeAlbumsSingleDayOutput, fileContent);
        }

        [TestMethod]
        public void TestFiveAlbumsTwoDays()
        {
            AlbumsPlayedExporter.ExportToFile(
                new List<AlbumPlayed> { Album_0, Album_1, Album_2, Album_3, Album_4 }, 
                out var fileContent);

            Assert.IsNotNull(fileContent);
            Assert.AreEqual(FiveAlbumsTwoDaysOutput, fileContent);
        }

        #endregion

        #region Constructor

        public AlbumsPlayedExporterTest()
        {
            TestFilePathAllDays = PathUtilities.GetPathToFile(PathUtilities.TestFileNameAllDays);
        }

        #endregion
    }
}
