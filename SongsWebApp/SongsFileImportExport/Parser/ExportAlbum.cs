namespace SongsFileImportExport.Parser
{
    using System;


    public class ExportAlbum
    {
        public string id { get; set; }
        public DateTime date { get; set; }
        public string location { get; set; }
        public string artist { get; set; }
        public string album { get; set; }
        public string userName { get; set; }
        public string imagePath { get; set; }
        public string playerLink { get; set; }

        public ExportAlbum()
        {
            id = string.Empty;
            date = DateTime.Now;
            location = string.Empty;
            artist = string.Empty;
            album = string.Empty;
            userName = string.Empty;
            imagePath = string.Empty;
            playerLink = string.Empty;

        }
    }
}
