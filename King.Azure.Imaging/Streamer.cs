namespace King.Azure.Imaging
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using King.Azure.Data;

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
        /// Get File Stream
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Stream</returns>
        public virtual async Task<Stream> Stream(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }

            var properties = await container.Properties(file);
            this.MimeType = properties.ContentType;

            return await container.Stream(file);
        }

        /// <summary>
        /// Get File Bytes
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Byte[]</returns>
        public virtual async Task<byte[]> Bytes(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentException("file");
            }

            var exists = await this.container.Exists(file);
            if (exists)
            {
                var properties = await container.Properties(file);
                this.MimeType = properties.ContentType;

                return await container.Get(file);
            }

            return null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Mime Type
        /// </summary>
        public virtual string MimeType
        {
            private set;
            get;
        }
        #endregion
    }
}