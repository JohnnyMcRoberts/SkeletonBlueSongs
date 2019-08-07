namespace SongsBurger.Controllers
{
    using System.IO;
    using System.Net.Http.Headers;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Settings;

    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private readonly FileUploadSettings _uploaderSettings;

        [HttpPost("{key}"), DisableRequestSizeLimit]
        public ActionResult UploadFile(string key)
        {
            try
            {
                var file = Request.Form.Files[0];

                string uploadRootPath = _uploaderSettings.UploadFilesDirectory;
                string newPath = Path.Combine(uploadRootPath, key);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

        public FileUploadController(IOptions<FileUploadSettings> uploaderConfig)
        {
            // Get the settings.
            _uploaderSettings = uploaderConfig.Value;

        }
    }
}
