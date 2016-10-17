namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Data Store
    /// </summary>
    public class DataStore : IDataStore
    {
        #region Members
        /// <summary>
        /// Blob Container
        /// </summary>
        protected readonly IContainer container = null;

        /// <summary>
        /// Table
        /// </summary>
        protected readonly ITableStorage table = null;

        /// <summary>
        /// Storage Queue
        /// </summary>
        private readonly IStorageQueue queue = null;

        /// <summary>
        /// Imaging
        /// </summary>
        protected readonly IImaging imaging = null;

        /// <summary>
        /// Image Naming
        /// </summary>
        protected readonly INaming naming = null;

        /// <summary>
        /// Cache Control Duration
        /// </summary>
        protected readonly uint cacheControlDuration = 31536000;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public DataStore(string connectionString, uint cacheControlDuration = 31536000)
            : this(connectionString, new StorageElements(), cacheControlDuration)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public DataStore(string connectionString, IStorageElements elements, uint cacheControlDuration = 31536000)
            : this(new Imaging(), new Container(elements.Container, connectionString), new TableStorage(elements.Table, connectionString), new StorageQueue(elements.Queue, connectionString), new Naming(), cacheControlDuration)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public DataStore(IImaging imaging, IContainer container, ITableStorage table, IStorageQueue queue, INaming naming, uint cacheControlDuration = 31536000)
        {
            if (null == imaging)
            {
                throw new ArgumentNullException("imaging");
            }
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }
            if (null == table)
            {
                throw new ArgumentNullException("table");
            }
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }
            if (null == naming)
            {
                throw new ArgumentNullException("naming");
            }

            this.imaging = imaging;
            this.container = container;
            this.table = table;
            this.queue = queue;
            this.naming = naming;
            this.cacheControlDuration = cacheControlDuration < 0 ? 31536000 : cacheControlDuration;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Streamer
        /// </summary>
        public virtual IStreamer Streamer
        {
            get
            {
                return new Streamer(this.container);
            }
        }

        /// <summary>
        /// Cache Control Duration
        /// </summary>
        public virtual uint CacheDuration
        {
            get
            {
                return this.cacheControlDuration;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="content">Content</param>
        /// <param name="version">Version</param>
        /// <param name="mimeType">Mime Type</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="queueForResize">Queue For Resize</param>
        /// <param name="extension">Extension</param>
        /// <param name="quality">Quality</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Relative Path</returns>
        public virtual async Task<string> Save(string fileName, byte[] content, string version, string mimeType, Guid identifier
            , bool queueForResize = false, string extension = null, byte quality = Imaging.DefaultImageQuality, ushort width = 0
            , ushort height = 0)
        {
            fileName = fileName.ToLowerInvariant();
            version = version.ToLowerInvariant();
            mimeType = mimeType.ToLowerInvariant();

            //Store in Blob
            await this.container.Save(fileName, content, mimeType);
            await this.container.SetCacheControl(fileName, this.cacheControlDuration);

            if (0 >= width || 0 >= height)
            {
                var size = this.imaging.Size(content);
                width = (ushort)size.Width;
                height = (ushort)size.Height;
            }

            var ie = new ImageEntity
            {
                PartitionKey = identifier.ToString(),
                RowKey = version,
                FileName = fileName,
                MimeType = mimeType,
                FileSize = (uint)content.LongLength,
                Width = width,
                Height = height,
                Quality = quality,
                RelativePath = this.naming.RelativePath(container.Name, fileName),
            };

            await this.table.InsertOrReplace(ie);
            
            if (queueForResize)
            {
                await this.queue.Send(new ImageQueued
                {
                    Identifier = identifier,
                    FileNameFormat = this.naming.FileNamePartial(identifier),
                    OriginalExtension = extension,
                });
            }

            return ie.RelativePath;
        }

        /// <summary>
        /// Resize Image
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="format">Format</param>
        /// <param name="quality">Quality</param>
        /// <param name="cache">Cache</param>
        /// <returns>Image Data</returns>
        public async Task<ImageData> Resize(string file, ushort width, ushort height = 0, string format = Naming.DefaultExtension
            , byte quality = Imaging.DefaultImageQuality, bool cache = true)
        {
            var wasCached = false;
            var imgFormat = this.imaging.Get(format, quality);

            var data = new ImageData()
            {
                MimeType = imgFormat.MimeType,
            };

            var identifier = this.naming.FromFileName(file);
            var versionName = this.naming.DynamicVersion(imgFormat.DefaultExtension, quality, width, height);
            var cachedFileName = this.naming.FileName(identifier, versionName, imgFormat.DefaultExtension);

            var streamer = this.Streamer;

            if (cache)
            {
                data.Raw = await streamer.Bytes(cachedFileName);
                wasCached = null != data.Raw;
            }

            if (!wasCached)
            {
                var version = new ImageVersion
                {
                    Height = height,
                    Width = width,
                    Format = imgFormat,
                };

                var toResize = await streamer.Bytes(file);
                data.Raw = this.imaging.Resize(toResize, version);
            }

            if (cache && !wasCached)
            {
                await this.Save(cachedFileName, data.Raw, versionName, imgFormat.MimeType, identifier, false, null, quality);
            }

            return data;
        }
        #endregion
    }
}