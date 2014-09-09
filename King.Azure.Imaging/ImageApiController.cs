namespace King.Azure.Imaging
{
    using ImageResizer;
    using King.Azure.Data;
    using System;
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
        /// Storage Elements
        /// </summary>
        protected readonly IStorageElements elements = null;

        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ImageApiController(string connectionString)
            : this(connectionString, new ImagePreprocessor(connectionString), new StorageElements())
        {

        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="preprocessor"></param>
        /// <param name="elements"></param>
        public ImageApiController(string connectionString, IImagePreprocessor preprocessor, IStorageElements elements)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }
            if (null == preprocessor)
            {
                throw new ArgumentException("preprocessor");
            }
            if (null == elements)
            {
                throw new ArgumentException("elements");
            }

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

        /// <summary>
        /// http://documentation.imageresizing.net/doxygen/class_image_resizer_1_1_instructions.html
        /// </summary>
        /// <returns>Image</returns>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Resize(string file, int width, int height, string format = "jpg")
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }
            if (0 >= width)
            {
                throw new ArgumentException("width");
            }
            if (0 >= height)
            {
                throw new ArgumentException("width");
            }
            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException("format");
            }

            var instructionSet = string.Format("width={0}&height={1}&format={2}", width, height, format);
            var streamer = new ImageStreamer(this.container);

            var response = new HttpResponseMessage();
            using (var input = await streamer.Get(file))
            {
                var resize = new MemoryStream();
                var job = new ImageJob(input, resize, new Instructions(instructionSet));
                job.Build();

                response.Content = new StreamContent(new MemoryStream(resize.ToArray()));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(job.ResultMimeType);
            }

            return response;
        }
        #endregion
    }
}