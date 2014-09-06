namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImageController : ApiController
    {
        #region Members
        /// <summary>
        /// Image Preprocessor
        /// </summary>
        private readonly IImagePreprocessor preprocessor = new ImagePreprocessor("UseDevelopmentStorage=true");
        #endregion

        #region Methods
        [HttpPost]
        public async Task Post()
        {
            var request = HttpContext.Current.Request;
            var files = request.Files;
            if (null != files)
            {
                foreach (string index in files)
                {
                    var file = request.Files[index];
                    var contentType = file.ContentType;
                    var fileName = file.FileName;
                    var bytes = new byte[file.ContentLength];
                    await file.InputStream.ReadAsync(bytes, 0, bytes.Length);
                    await this.preprocessor.Process(bytes, contentType, fileName);
                }
            }
        }
        #endregion
    }
}