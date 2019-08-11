namespace SongsBurger.Controllers.Utilities
{
    using System.IO;

    public class FileUtilities
    {
        public const string SongDetailsFileName = "SongDetails.txt";

        public static string GetFullSongsDetailsFilePath(string key, string uploadRootPath)
        {
            string newPath = Path.Combine(uploadRootPath, key);

            string fileName = SongDetailsFileName;
            var files = Directory.GetFiles(newPath);
            if (files != null && files.Length > 0)
                fileName = files[0];

            string fullPath = Path.Combine(newPath, fileName);
            return fullPath;
        }
    }
}
