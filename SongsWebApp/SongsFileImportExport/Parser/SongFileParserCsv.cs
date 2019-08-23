using CsvHelper;

namespace SongsFileImportExport.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using SongsDatabase.DataModels;

    public static class SongFileParserCsv
    {
        public static List<AlbumPlayed> GetAlbumsPlayedFromFile(string filePath)
        {
            List<AlbumPlayed> albums = new List<AlbumPlayed>();

            // check file exists
            if (!File.Exists(filePath))
                return albums;

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.BadDataFound = null;
                var records = csv.GetRecords<ExportAlbum>();
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
            }

            return albums;
        }
    }

}
