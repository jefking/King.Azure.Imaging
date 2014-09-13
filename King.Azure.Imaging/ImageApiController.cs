namespace King.Azure.Imaging
{
    using ImageProcessor;
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net;
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
        protected readonly IImagePreprocessor preprocessor = null;

        /// <summary>
        /// Streamer
        /// </summary>
        protected readonly IImageStreamer streamer = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageApiController(string connectionString)
            : this(connectionString, new ImagePreprocessor(connectionString), new StorageElements())
        {

        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageApiController(string connectionString, IImagePreprocessor preprocessor, IStorageElements elements)
            : this(preprocessor, new ImageStreamer(new Container(elements.Container, connectionString)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageApiController(IImagePreprocessor preprocessor, IImageStreamer streamer)
        {
            if (null == preprocessor)
            {
                throw new ArgumentException("preprocessor");
            }
            if (null == streamer)
            {
                throw new ArgumentException("streamer");
            }

            this.preprocessor = preprocessor;
            this.streamer = streamer;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Post new file to the server
        /// </summary>
        /// <returns>Task</returns>
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

        /// <summary>
        /// Get a specific file from Blob storage
        /// </summary>
        /// <param name="file">file name</param>
        /// <returns>File</returns>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Get(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return new HttpResponseMessage(HttpStatusCode.PreconditionFailed)
                {
                    ReasonPhrase = "file must be specified",
                };
            }

            var ms = await this.streamer.Get(file);
            var response = new HttpResponseMessage();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(this.streamer.ContentType);
            return response;
        }

        /// <summary>
        /// Resize Image on the fly
        /// </summary>
        /// <returns>Image (Resized)</returns>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Resize(string file, int width, int height = 0)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return new HttpResponseMessage(HttpStatusCode.PreconditionFailed)
                {
                    ReasonPhrase = "file must be specified",
                };
            }
            if (0 > width)
            {
                return new HttpResponseMessage(HttpStatusCode.PreconditionFailed)
                {
                    ReasonPhrase = "width less than 0",
                };
            }
            if (0 > height)
            {
                return new HttpResponseMessage(HttpStatusCode.PreconditionFailed)
                {
                    ReasonPhrase = "height less than 0",
                };
            }
            if (0 >= width && 0 >= height)
            {
                return new HttpResponseMessage(HttpStatusCode.PreconditionFailed)
                {
                    ReasonPhrase = "width and heigh less than or equal to 0",
                };
            }

            var response = new HttpResponseMessage();
            using (var input = await this.streamer.Get(file))
            {
                var resize = new MemoryStream();
                var jpg = new JpegFormat { Quality = 70 };//Make Dynamic
                var size = new Size(width, height);
                using (var imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(input)
                                .Resize(size)
                                .Format(jpg)//Make Dynamic
                                .Save(resize);
                }

                response.Content = new StreamContent(new MemoryStream(resize.ToArray()));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(jpg.MimeType);
            }

            return response;
        }
        #endregion
    }
}