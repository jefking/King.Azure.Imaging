namespace King.Azure.Imaging
{
    using King.Azure.Imaging.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Image Api Controller
    /// </summary>
    public class ImageApi : ApiController
    {
        #region Members
        /// <summary>
        /// Image Preprocessor
        /// </summary>
        protected readonly IPreprocessor preprocessor = null;

        /// <summary>
        /// Image Store
        /// </summary>
        protected readonly IDataStore store = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageApi(string connectionString)
            : this(connectionString, new Preprocessor(connectionString), new StorageElements())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageApi(string connectionString, IPreprocessor preprocessor, IStorageElements elements)
            : this(preprocessor, new DataStore(connectionString, elements))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageApi(IPreprocessor preprocessor, IDataStore store)
        {
            if (null == preprocessor)
            {
                throw new ArgumentException("preprocessor");
            }
            if (null == store)
            {
                throw new ArgumentException("store");
            }

            this.preprocessor = preprocessor;
            this.store = store;
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

            var ids = new List<string>(provider.Contents.Count);
            foreach (var file in provider.Contents)
            {
                var bytes = await file.ReadAsByteArrayAsync();

                var id = await this.preprocessor.Process(bytes, file.Headers.ContentType.MediaType, file.Headers.ContentDisposition.FileName.Trim('\"'));

                ids.Add(id);
            }

            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"),
            };
        }

        /// <summary>
        /// Get a specific file from Blob storage; resize as needed
        /// </summary>
        /// <returns>Image (Resized)</returns>
        [HttpGet]
        public virtual async Task<HttpResponseMessage> Get(string file, ushort width = 0, ushort height = 0, string format = Naming.DefaultExtension, byte quality = Imaging.DefaultImageQuality, bool cache = true)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return new HttpResponseMessage(HttpStatusCode.PreconditionFailed)
                {
                    ReasonPhrase = "file must be specified",
                };
            }

            string mimeType = null;
            Stream stream = null;

            if (0 == width && 0 == height)
            {
                var streamer = this.store.Streamer;
                stream = await streamer.Stream(file);
                mimeType = streamer.MimeType;
            }
            else
            {
                var data = await this.store.Resize(file, width, height, format, quality, cache);
                stream = new MemoryStream(data.Raw);
                mimeType = data.MimeType;
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream),
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            return response;
        }
        #endregion
    }
}