namespace SongsFileImportExport.Exporter
{
    using System;
    using System.Collections.Generic;

    using SongsDatabase.DataModels;

    public static class AlbumsPlayedExporter
    {
        #region Public Methods

        public static void ExportToFile(List<AlbumPlayed> albums, out string fileContent)
        {
            fileContent = string.Empty;

            AlbumPlayed currentAlbumPlayed = new AlbumPlayed { Date = DateTime.MinValue };

            foreach (var album in albums)
            {
                if (currentAlbumPlayed.Date.DayOfYear != album.Date.DayOfYear ||
                    currentAlbumPlayed.Date.Year != album.Date.Year)
                {
                    WriteNewDate(album.Date, ref fileContent);
                    currentAlbumPlayed.Date = album.Date;
                    currentAlbumPlayed.Location = string.Empty;
                    currentAlbumPlayed.Artist = string.Empty;
                }

                if (currentAlbumPlayed.Location != album.Location)
                {
                    WriteNewLocation(album.Location, ref fileContent);
                    currentAlbumPlayed.Location = album.Location;
                    currentAlbumPlayed.Artist = string.Empty;
                }

                if (currentAlbumPlayed.Artist != album.Artist)
                {
                    WriteNewArtist(album.Artist, ref fileContent);
                    currentAlbumPlayed.Artist = album.Artist;
                }

                WriteNewAlbum(album.Album, ref fileContent);
            }

        }

        private static void WriteNewAlbum(string albumAlbum, ref string fileContent)
        {
            fileContent += "\t\t\t" + albumAlbum + "\n";
        }

        private static void WriteNewArtist(string albumArtist, ref string fileContent)
        {
            fileContent += "\n\t\t" + albumArtist + "\n";
        }

        private static void WriteNewLocation(string albumLocation, ref string fileContent)
        {
            fileContent += "\n\t" + albumLocation + "\n";
        }

        private static void WriteNewDate(DateTime albumDate, ref string fileContent)
        {
            fileContent += 
                "\n" + 
                albumDate.ToString("dddd d") +
                GetDaySuffix (albumDate.Day) +
                albumDate.ToString(" MMMM") +
                " " + 
                albumDate.Year + 
                "\n";
        }

        private static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }
        #endregion
    }
}
