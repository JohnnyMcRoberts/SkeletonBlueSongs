using CsvHelper;

namespace SongsFileImportExport.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using SongsDatabase.DataModels;

    public static class SongFileParserJson
    {
        public static List<AlbumPlayed> GetAlbumsPlayedFromFile(string filePath)
        {
            List<AlbumPlayed> albums = new List<AlbumPlayed>();

            // check file exists
            if (!File.Exists(filePath))
                return albums;

            string text = File.ReadAllText(filePath);

            var records = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportAlbum[]>(text);

            foreach (var record in records)
            {
                albums.Add(
                    new AlbumPlayed
                    {
                        Album = record.album,
                        Artist = record.artist,
                        Date = record.date,
                        ImagePath = record.imagePath,
                        Location = record.location,
                        PlayerLink = record.playerLink,
                        UserName = record.userName
                    });
            }

            return albums;
        }
    }

}
