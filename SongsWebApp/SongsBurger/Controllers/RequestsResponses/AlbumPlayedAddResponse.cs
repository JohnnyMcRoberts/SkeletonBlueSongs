namespace SongsBurger.Controllers.RequestsResponses
{
    using System;

    using SongsDatabase.DataModels;

    public class DisplayAlbum
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
        public string PlayerLink { get; set; }

        public DisplayAlbum(AlbumPlayed displayAlbum)
        {
            Id = displayAlbum.Id.ToString();
            Date = displayAlbum.Date;
            Location = displayAlbum.Location;
            Artist = displayAlbum.Artist;
            Album = displayAlbum.Album;
            UserName = displayAlbum.UserName;
            ImagePath = displayAlbum.ImagePath;
            PlayerLink = displayAlbum.PlayerLink;
        }

        public DisplayAlbum(DisplayAlbum displayAlbum)
        {
            Id = displayAlbum.Id;
            Date = displayAlbum.Date;
            Location = displayAlbum.Location;
            Artist = displayAlbum.Artist;
            Album = displayAlbum.Album;
            UserName = displayAlbum.UserName;
            ImagePath = displayAlbum.ImagePath;
            PlayerLink = displayAlbum.PlayerLink;
        }

        public DisplayAlbum()
        {
            Id = string.Empty;
            Date = DateTime.Now;
            Location = string.Empty;
            Artist = string.Empty;
            Album = string.Empty;
            UserName = string.Empty;
            ImagePath = string.Empty;
            PlayerLink = string.Empty;
        }
    }

    public class AlbumPlayedAddResponse
    {
        public DisplayAlbum Album { get; set; }

        public int ErrorCode { get; set; }

        public string FailReason { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public AlbumPlayedAddResponse()
        {
            Album = new DisplayAlbum();
            ErrorCode = (int)UserResponseCode.Success;
            FailReason = string.Empty;
            UserId = string.Empty;
            Description = string.Empty;
            Email = string.Empty;
        }
    }
}
