namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class ImageStore : IImageStore
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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public ImageStore(string connectionString)
            : this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageStore(string connectionString, IStorageElements elements)
            : this(new Imaging(), new Container(elements.Container, connectionString), new TableStorage(elements.Table, connectionString), new StorageQueue(elements.Queue, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageStore(IImaging imaging, IContainer container, ITableStorage table, IStorageQueue queue)
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

            this.imaging = imaging;
            this.container = container;
            this.table = table;
            this.queue = queue;
        }
        #endregion

        #region Methods
        public async Task Save(string fileName, byte[] content, string version, string mimeType, Guid identifier, bool queueForResize = false, string extension = null, int quality = 100, int width = 0, int height = 0)
        {
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
                RelativePath = string.Format("{0}/{1}", container.Name, fileName),
            });

            if (queueForResize)
            {
                //Queue for Processing
                var json = JsonConvert.SerializeObject(new ImageQueued
                {
                    Identifier = identifier,
                    FileNameFormat = string.Format(ImagePreprocessor.FileNameFormat, identifier, "{0}", "{1}"),
                    OriginalExtension = extension,
                });
                await this.queue.Save(new CloudQueueMessage(json));
            }
        }
        #endregion
    }
}