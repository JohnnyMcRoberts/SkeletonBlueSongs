using System.Linq;

namespace SongsFileImportExport.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using SongsDatabase.DataModels;

    public enum FileReadState
    {
        Reset,
        OnDate,
        OnLocation,
        OnArtist
    };

    public static class SongFileParser
    {
        #region Constants

        public const int NumberWordsInDate = 4;
        public static readonly string[] DayStrings = 
            {"monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"};

        public static readonly string[] OrdinalStrings =
            {"st", "nd", "rd", "th"};


        public static readonly char[] IgnoreChars = {' ', '\t'};

        public static readonly Dictionary<string, int> MonthStringsToNumbers = 
            new Dictionary<string, int>
            {
                {"january", 1},
                {"february", 2},
                {"march", 3},

                {"april", 4},
                {"may", 5},
                {"june", 6},

                {"july", 7},
                {"august", 8},
                {"september", 9},

                {"october", 10},
                {"november", 11},
                {"december", 12}
            };

        #endregion

        #region Private Functions

        private static void ProcessLine(
            string line,
            ref FileReadState state,
            ref List<AlbumPlayed> albums,
            ref AlbumPlayed currentAlbumPlayed)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            switch (state)
            {
                case FileReadState.Reset:
                    ParseLineWhenReset(line, ref state, ref currentAlbumPlayed);
                    break;
                case FileReadState.OnDate:
                    ParseLineWhenOnDate(line, ref state, ref currentAlbumPlayed);
                    break;
                case FileReadState.OnLocation:
                    ParseLineWhenOnLocation(line, ref state, ref currentAlbumPlayed);
                    break;
                case FileReadState.OnArtist:
                    ParseLineWhenOnArtist(line, ref state, ref currentAlbumPlayed, albums);
                    break;

            }
        }

        private static void ParseLineWhenOnArtist(
            string line, 
            ref FileReadState state,
            ref AlbumPlayed currentAlbumPlayed,
            List<AlbumPlayed> albums)
        {
            if (line[0] == '\t' && line[1] == '\t' && line[2] == '\t')
            {
                string album = line.TrimEnd(IgnoreChars).TrimStart(IgnoreChars);
                currentAlbumPlayed.Album = album;
                albums.Add(currentAlbumPlayed);
                currentAlbumPlayed = new AlbumPlayed(currentAlbumPlayed);
            }
            else if (line[0] == '\t' && line[1] == '\t')
            {
                ParseLineWhenOnLocation(line, ref state, ref currentAlbumPlayed);
            }
            else if (line[0] == '\t')
            {
                ParseLineWhenOnDate(line, ref state, ref currentAlbumPlayed);

            }
            else
            {
                ParseLineWhenReset(line, ref state, ref currentAlbumPlayed);
            }
        }

        private static void ParseLineWhenOnLocation(string line, ref FileReadState state, ref AlbumPlayed currentAlbumPlayed)
        {
            if (line[0] == '\t' && line[1] == '\t')
            {
                string artist = line.TrimEnd(IgnoreChars).TrimStart(IgnoreChars);
                currentAlbumPlayed.Artist = artist;
                state = FileReadState.OnArtist;
            }
        }

        private static void ParseLineWhenOnDate(string line, ref FileReadState state, ref AlbumPlayed currentAlbumPlayed)
        {
            if (line[0] == '\t')
            {
                string location = line.TrimEnd(IgnoreChars).TrimStart(IgnoreChars);
                currentAlbumPlayed.Location = location;
                state = FileReadState.OnLocation;
            }
        }

        private static void ParseLineWhenReset(string line, ref FileReadState state, ref AlbumPlayed currentAlbumPlayed)
        {
            if (char.IsLetterOrDigit(line[0]))
            {
                var words = line.Split(' ');
                if (words.Length >= NumberWordsInDate)
                {
                    DateTime date = DateTime.Now;
                    if (ParseDateFromString(words, ref date))
                    {
                        currentAlbumPlayed.Date = date;
                        state = FileReadState.OnDate;
                    }
                }
            }
        }

        private static bool ParseDateFromString(string[] words, ref DateTime date)
        {
            // example text would be "Monday 26th February 2018"
            if (DayStrings.Contains(words[0].ToLower()))
            {
                var dayOfMonthString = words[1].ToLower();
                var monthString = words[2].ToLower();
                var yearString = words[3].ToLower();

                int dayOfMonth = 1;
                if (!EndsWithOrdinal(dayOfMonthString, ref dayOfMonth))
                {
                    return false;
                }

                if (!MonthStringsToNumbers.ContainsKey(monthString))
                {
                    return false;
                }

                int monthOfYear = MonthStringsToNumbers[monthString];

                if (!int.TryParse(yearString, out var year) || year < 2015)
                {
                    return false;
                }

                date = new DateTime(year, monthOfYear, dayOfMonth, 12, 0, 0);
                return true;
            }

            return false;
        }

        private static bool EndsWithOrdinal(string dayOfMonth, ref int day)
        {
            bool endsOk = false;
            foreach (var ordinal in OrdinalStrings)
            {
                if(dayOfMonth.EndsWith(ordinal))
                    endsOk = true;
            }

            if (endsOk)
            {
                string dayNumbers = dayOfMonth.Remove(dayOfMonth.Length - 2);
                if (int.TryParse(dayNumbers, out day))
                {
                    return true;
                }

            }

            return false;
        }

        #endregion

        #region Public Methods

        public static List<AlbumPlayed> GetAlbumsPlayedFromFile(string filePath)
        {
            List<AlbumPlayed> albums = new List<AlbumPlayed>();

            // check file exists
            if (!File.Exists(filePath))
                return albums;

            // read the file line by line
            string line;
            FileReadState parseState = FileReadState.Reset;
            AlbumPlayed currentAlbumPlayed  = new AlbumPlayed();

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                ProcessLine(line, ref parseState, ref albums, ref currentAlbumPlayed);
            }

            file.Close();

            return albums;
        }

        #endregion
    }
}
