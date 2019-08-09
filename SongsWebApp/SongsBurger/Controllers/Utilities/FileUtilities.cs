using System.IO;

namespace SongsBurger.Controllers.Utilities
{
    public class FileUtilities
    {
        public const string SongDetailsFileName = "SongDetails.txt";

        public static string GetFullSongsDetailsFilePath(string key, string uploadRootPath)
        {
            string newPath = Path.Combine(uploadRootPath, key);
            string fileName = SongDetailsFileName;
            string fullPath = Path.Combine(newPath, fileName);
            return fullPath;
        }
    }
}
