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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public DataStore(string connectionString)
            : this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public DataStore(string connectionString, IStorageElements elements)
            : this(new Imaging(), new Container(elements.Container, connectionString), new TableStorage(elements.Table, connectionString), new StorageQueue(elements.Queue, connectionString), new Naming())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public DataStore(IImaging imaging, IContainer container, ITableStorage table, IStorageQueue queue, INaming naming)
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
        #endregion

        #region Methods
        public virtual async Task Save(string fileName, byte[] content, string version, string mimeType, Guid identifier, bool queueForResize = false
            , string extension = null, int quality = 100, int width = 0, int height = 0)
        {
            fileName = fileName.ToLowerInvariant();
            version = version.ToLowerInvariant();
            mimeType = mimeType.ToLowerInvariant();

            //Store in Blob
            await this.container.Save(fileName, content, mimeType);

            if (0 >= width || 0 >= height)
            {
                var size = this.imaging.Size(content);
                width = size.Width;
                height = size.Height;
            }

            //Store in Table
            await this.table.InsertOrReplace(new ImageEntity
            {
                PartitionKey = identifier.ToString(),
                RowKey = version,
                FileName = fileName,
                ContentType = mimeType,
                FileSize = content.LongLength,
                Width = width,
                Height = height,
                Quality = quality,
                RelativePath = this.naming.RelativePath(container.Name, fileName),
            });

            if (queueForResize)
            {
                //Queue for Processing
                await this.queue.Save(new ImageQueued
                {
                    Identifier = identifier,
                    FileNameFormat = this.naming.FileNamePartial(identifier),
                    OriginalExtension = extension,
                });
            }
        }

        public async Task<ImageData> Resize(string file, int width, int height = 0, string format = Naming.DefaultExtension, int quality = 85, bool cache = true)
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
                data.Raw = await streamer.GetBytes(cachedFileName);
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

                var toResize = await streamer.GetBytes(file);
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