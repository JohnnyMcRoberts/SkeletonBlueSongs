using SongsDatabase.DataModels;

namespace SongsBurger.Controllers.RequestsResponses
{
    public class AlbumPlayedAddResponse
    {
        public AlbumPlayed Album { get; set; }

        public int ErrorCode { get; set; }

        public string FailReason { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public AlbumPlayedAddResponse()
        {
            Album = new AlbumPlayed();
            ErrorCode = (int)UserResponseCode.Success;
            FailReason = string.Empty;
            UserId = string.Empty;
            Description = string.Empty;
            Email = string.Empty;
        }
    }
}
