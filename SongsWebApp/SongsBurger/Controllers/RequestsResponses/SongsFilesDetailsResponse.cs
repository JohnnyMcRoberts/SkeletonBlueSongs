namespace SongsBurger.Controllers.RequestsResponses
{
    using SongsDatabase.DataModels;

    public enum SongsFilesResponseCode
    {
        Success = 0,
        InvalidFile,
        NoSongsInFile,
        InvalidUser
    };

    public class SongsFilesDetailsResponse
    {
        public string FileName { get; set; }

        public int ErrorCode { get; set; }

        public string FailReason { get; set; }

        public AlbumPlayed[] AlbumsPlayed { get; set; }

        public SongsFilesDetailsResponse()
        {
            FileName = string.Empty;
            ErrorCode = (int) SongsFilesResponseCode.Success;
            FailReason = string.Empty;
            AlbumsPlayed = new AlbumPlayed[] { };
        }

        public SongsFilesDetailsResponse(SongsFilesDetailsResponse request)
        {
            FileName = request.FileName;
            ErrorCode = request.ErrorCode;
            FailReason = request.FailReason;

            AlbumsPlayed = new AlbumPlayed[request.AlbumsPlayed.Length];
            for (int i = 0; i < request.AlbumsPlayed.Length; i++)
            {
                AlbumsPlayed[i] = new AlbumPlayed(request.AlbumsPlayed[i]);
            }
        }
    }
}
