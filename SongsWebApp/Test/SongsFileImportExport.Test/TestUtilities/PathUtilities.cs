namespace SongsFileImportExport.Test.TestUtilities
{
    using System;
    using System.IO;

    public static class PathUtilities
    {
        #region Constants

        public const string TestFileNameAllDays = "AllSongsToDate.txt";
        public const string TestFileNameMultipleDaysSongs = "MultipleDaysSongs.txt";
        public const string TestFileNameMultipleSongsSingleDay = "MultipleSongsSingleDay.txt";
        public const string TestFileNameSingleAlbum = "SingleAlbum.txt";
        public const string TestFileNameSingleDay = "SingleDay.txt";

        public const string TestFileNameAllDaysCsv = "AllSongsToDate.csv";
        public const string TestFileNameSingleDayCsv = "SingleDay.csv";

        public const string TestFileNameAllDaysJson = "AllSongsToDate.json";
        public const string TestFileNameSingleDayJson = "SingleDay.json";

        #endregion

        public static string GetPathToFile(string filename)
        {
            try
            {
                string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                return Path.Combine(projectPath, "Assets", filename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
