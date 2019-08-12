namespace SongsBurger.Controllers.RequestsResponses
{
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

        public DisplayAlbum[] AlbumsPlayed { get; set; }

        public SongsFilesDetailsResponse()
        {
            FileName = string.Empty;
            ErrorCode = (int) SongsFilesResponseCode.Success;
            FailReason = string.Empty;
            AlbumsPlayed = new DisplayAlbum[] { };
        }

        public SongsFilesDetailsResponse(SongsFilesDetailsResponse request)
        {
            FileName = request.FileName;
            ErrorCode = request.ErrorCode;
            FailReason = request.FailReason;

            AlbumsPlayed = new DisplayAlbum[request.AlbumsPlayed.Length];
            for (int i = 0; i < request.AlbumsPlayed.Length; i++)
            {
                AlbumsPlayed[i] = new DisplayAlbum(request.AlbumsPlayed[i]);
            }
        }
    }
}
