namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using King.Azure.Data;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
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
        /// Connection String
        /// </summary>
        private const string connectionString = "UseDevelopmentStorage=true";

        /// <summary>
        /// Image Preprocessor
        /// </summary>
        private readonly IImagePreprocessor preprocessor = new ImagePreprocessor(connectionString);

        /// <summary>
        /// Storage Elements
        /// </summary>
        private static readonly IStorageElements elements = new StorageElements();
        #endregion

        #region Methods
        [HttpPost]
        public async Task Post()
        {
            var request = HttpContext.Current.Request;
            if (null != request.Files)
            {
                foreach (string index in request.Files)
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

        [HttpGet]
        public async Task<HttpResponseMessage> Get(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }

            var streamer = new ImageStreamer(new Container(elements.Container, connectionString));
            var ms = await streamer.Get(file);
            var response = new HttpResponseMessage();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(streamer.ContentType);
            return response;
        }
        #endregion
    }
}