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

        public const string TestFileNameAllDays = "listening in.txt";
        public const string TestFileNameMultipleDaysSongs = "MultipleDaysSongs.txt";
        public const string TestFileNameMultipleSongsSingleDay = "MultipleSongsSingleDay.txt";
        public const string TestFileNameSingleAlbum = "SingleAlbum.txt";
        public const string TestFileNameSingleDay = "SingleDay.txt";
        public readonly string TestFilePathAllDays;

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetPathToFile()
        {
            Assert.IsTrue(File.Exists(TestFilePathAllDays));
        }
        
        #endregion

        #region Constructor

        public AlbumsPlayedExporterTest()
        {
            TestFilePathAllDays = PathUtilities.GetPathToFile(TestFileNameAllDays);
        }

        #endregion
    }
}
