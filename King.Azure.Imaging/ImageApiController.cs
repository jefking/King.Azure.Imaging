namespace King.Azure.Imaging
{
    using ImageProcessor;
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using System;
    using System.Drawing;
    using System.IO;
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

            var ms = await this.streamer.Get(file);
            var response = new HttpResponseMessage();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(this.streamer.ContentType);
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Image</returns>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Resize(string file, int width = 0, int height = 0)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }
            if (0 > width)
            {
                throw new ArgumentException("width");
            }
            if (0 > height)
            {
                throw new ArgumentException("width");
            }
            if (0 >= width && 0 >= height)
            {
                throw new ArgumentException("width and height <= 0.");
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