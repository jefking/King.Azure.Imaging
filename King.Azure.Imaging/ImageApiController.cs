namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    /// <summary>
    /// Image Api Controller
    /// </summary>
    public class ImageApiController : ApiController
    {
        #region Members
        /// <summary>
        /// Image Preprocessor
        /// </summary>
        private readonly IImagePreprocessor preprocessor = null;

        /// <summary>
        /// Storage Elements
        /// </summary>
        private readonly IStorageElements elements = null;

        /// <summary>
        /// Container
        /// </summary>
        private readonly IContainer container = null;
        #endregion

        #region Constructors]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public ImageApiController(string connectionString)
            : this(connectionString, new ImagePreprocessor(connectionString), new StorageElements())
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="preprocessor"></param>
        /// <param name="elements"></param>
        public ImageApiController(string connectionString, IImagePreprocessor preprocessor, IStorageElements elements)
        {
            this.container = new Container(elements.Container, connectionString);
            this.preprocessor = preprocessor;
            this.elements = elements;
        }
        #endregion

        #region Methods
        [HttpPost]
        public virtual async Task Post()
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
        public virtual async Task<HttpResponseMessage> Get(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }

            var streamer = new ImageStreamer(this.container);
            var ms = await streamer.Get(file);
            var response = new HttpResponseMessage();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(streamer.ContentType);
            return response;
        }
        #endregion
    }
}