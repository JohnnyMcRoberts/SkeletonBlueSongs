namespace SongsApi.Controllers.RequestsResponses
{
    public class SongsFilesDetailsRequest
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public SongsFilesDetailsRequest()
        {
            FileName = string.Empty;
            FilePath = string.Empty;
        }

        public SongsFilesDetailsRequest(SongsFilesDetailsRequest request)
        {
            FileName = request.FileName;
            FilePath = request.FilePath;
        }
    }
}
