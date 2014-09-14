namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Preprocessor
    /// </summary>
    public class ImagePreprocessor : IImagePreprocessor
    {
        #region Members
        /// <summary>
        /// Original
        /// </summary>
        public const string Original = "original";

        /// <summary>
        /// File Name Format
        /// </summary>
        public const string FileNameFormat = "{0}_{1}{2}";

        /// <summary>
        /// Path Format
        /// </summary>
        public const string PathFormat = "{0}/{1}";

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
        protected readonly IStorageQueue queue = null;

        /// <summary>
        /// Imaging
        /// </summary>
        protected readonly Imaging img = new Imaging();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public ImagePreprocessor(string connectionString)
            :this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImagePreprocessor(string connectionString, IStorageElements elements)
            :this(new Container(elements.Container, connectionString), new TableStorage(elements.Table, connectionString), new StorageQueue(elements.Queue, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImagePreprocessor(IContainer container, ITableStorage table, IStorageQueue queue)
        {
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

            this.container = container;
            this.table = table;
            this.queue = queue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Preprocess uploaded image
        /// </summary>
        /// <param name="content">Content</param>
        /// <param name="contentType">Content Type</param>
        /// <param name="fileName">File Name</param>
        /// <returns>Task</returns>
        public virtual async Task Process(byte[] content, string contentType, string fileName)
        {
            if (null == content || !content.Any())
            {
                throw new ArgumentNullException("content");
            }
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException("contentType");
            }
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("fileName");
            }

            var id = Guid.NewGuid();
            var extension = fileName.Contains('.') ? fileName.Substring(fileName.LastIndexOf('.')) : ".jpg";
            var originalFileName = string.Format(FileNameFormat, id, Original, extension);

            //Store Blob
            await container.Save(originalFileName, content, contentType);

            var size = img.Size(content);

            //Store in Table
            await table.InsertOrReplace(new ImageEntity()
            {
                PartitionKey = id.ToString(),
                ContentType = contentType,
                RowKey = Original,
                RelativePath = string.Format(PathFormat, this.container.Name, originalFileName),
                FileSize = content.LongLength,
                FileName = originalFileName,
                Width = size.Width,
                Height = size.Height,
            });

            //Queue for Processing
            var toQueue = JsonConvert.SerializeObject(new ImageQueued()
            {
                Identifier = id,
                FileNameFormat = string.Format(FileNameFormat, id, "{0}", extension)
            });

            await this.queue.Save(new CloudQueueMessage(toQueue));
        }
        #endregion
    }
}