namespace SongsDatabase.DataModels
{
    using System;

    using MongoDB.Bson.Serialization.Attributes;

    using Base;


    [BsonIgnoreExtraElements]
    public class AlbumPlayed : BaseMongoEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the date that the album was listened on.
        /// </summary>
        [BsonElement("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the location that the album was listened in.
        /// </summary>
        [BsonElement("location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the artist played.
        /// </summary>
        [BsonElement("artist")]
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album played.
        /// </summary>
        [BsonElement("album")]
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        [BsonElement("user_name")]
        public string UserName { get; set; }

        #endregion

        #region BaseMongoEntity Implementation

        public override string EquivalenceName => Date.ToShortDateString() + " " +
                                                  Location + " " +
                                                  Artist + " " +
                                                  Album + " " +
                                                  UserName;

        #endregion

        #region  Constructors

        public AlbumPlayed()
        {
            Date = DateTime.Now;
            Location = string.Empty;
            Artist = string.Empty;
            Album = string.Empty;
            UserName = string.Empty;
        }

        public AlbumPlayed(AlbumPlayed album)
        {
            Date = album.Date;
            Location = album.Location;
            Artist = album.Artist;
            Album = album.Album;
            UserName = album.UserName;
        }

        #endregion 

    }
}
