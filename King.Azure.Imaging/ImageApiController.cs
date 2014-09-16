namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using System;
    using System.Linq;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
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

        /// <summary>
        /// Imaging
        /// </summary>
        protected readonly IImaging imaging = null;
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
            : this(preprocessor, new ImageStreamer(new Container(elements.Container, connectionString)), new Imaging())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageApiController(IImagePreprocessor preprocessor, IImageStreamer streamer, IImaging imaging)
        {
            if (null == preprocessor)
            {
                throw new ArgumentException("preprocessor");
            }
            if (null == streamer)
            {
                throw new ArgumentException("streamer");
            }
            if (null == imaging)
            {
                throw new ArgumentException("imaging");
            }

            this.preprocessor = preprocessor;
            this.streamer = streamer;
            this.imaging = imaging;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Post new file to the server
        /// </summary>
        /// <returns>Task</returns>
        [HttpPost]
        public virtual async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await this.Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var bytes = await file.ReadAsByteArrayAsync();

                await this.preprocessor.Process(bytes
                    , file.Headers.ContentType.MediaType
                    , file.Headers.ContentDisposition.FileName.Trim('\"'));
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
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

            var stream = await this.streamer.Get(file);
            var response = new HttpResponseMessage
            {
                Content = new StreamContent(stream),
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(this.streamer.ContentType);
            return response;
        }

        /// <summary>
        /// Resize Image on the fly
        /// </summary>
        /// <remarks>
        /// Format and Cache are not wired in yet, soon
        /// </remarks>
        /// <returns>Image (Resized)</returns>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Resize(string file, int width, int height = 0, string format = ImagePreprocessor.DefaultExtension, int quality = 85, bool cache = false)
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
                    ReasonPhrase = "width and height less than or equal to 0",
                };
            }

            var wasCached = false;

            var version = new ImageVersion()
            {
                Height = height,
                Width = width,
                Format = this.imaging.Get(format, quality),
            };

            var elements = new StorageElements();
            var table = new TableStorage(elements.Table, "UseDevelopmentStorage=true;");
            var partition = file.Substring(0, file.IndexOf('_'));
            var row = string.Format("{0}_{1}_{2}x{3}", version.Format.DefaultExtension, quality, width, height);
            var fileName = string.Format("{0}_{1}.{2}", partition, row, version.Format.DefaultExtension);

            byte[] resized = null;

            var response = new HttpResponseMessage();
            if (cache)
            {
                resized = await this.streamer.Get(Guid.Parse(partition), row, version.Format.DefaultExtension);
                if (null != resized)
                {
                    wasCached = true;

                    response.Content = new StreamContent(new MemoryStream(resized));
                }
            }

            if (null == response.Content)
            {
                using (var input = await this.streamer.Get(file))
                using (var ms = input as MemoryStream)
                {
                    resized = this.imaging.Resize(ms.ToArray(), version);
                    response.Content = new StreamContent(new MemoryStream(resized));
                }
            }

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(version.Format.MimeType);

            if (cache && !wasCached)
            {
                //Store in Blob
                var container = new Container(elements.Container, "UseDevelopmentStorage=true;");
                await container.Save(fileName, resized, version.Format.MimeType);

                //Store in Table
                await table.InsertOrReplace(new ImageEntity
                {
                    PartitionKey = partition,
                    RowKey = row,
                    FileName = fileName,
                    ContentType = version.Format.MimeType,
                    FileSize = resized.LongLength,
                    Width = width,
                    Height = height,
                    Quality = version.Format.Quality,
                    RelativePath = string.Format("{0}/{1}", container.Name, fileName),
                });
            }

            return response;
        }
        #endregion
    }
}