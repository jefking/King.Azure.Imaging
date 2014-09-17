namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Streamer
    /// </summary>
    public class Streamer : IStreamer
    {
        #region Members
        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Container</param>
        public Streamer(IContainer container)
        {
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get File
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Stream</returns>
        public virtual async Task<Stream> Get(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }

            var bytes = await container.Get(file);
            var ms = new MemoryStream();
            await ms.WriteAsync(bytes, 0, bytes.Length);
            ms.Position = 0;

            var properties = await container.Properties(file);
            this.ContentType = properties.ContentType;

            return ms;
        }

        /// <summary>
        /// Get File
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Byte[]</returns>
        public virtual async Task<byte[]> GetBytes(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }

            byte[] bytes = null;
            
            var exists = await this.container.Exists(file);
            if (exists)
            {
                bytes = await container.Get(file);

                var properties = await container.Properties(file);
                this.ContentType = properties.ContentType;
            }

            return bytes;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Content Type
        /// </summary>
        public virtual string ContentType
        {
            private set;
            get;
        }
        #endregion
    }
}