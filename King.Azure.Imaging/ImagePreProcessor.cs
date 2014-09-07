namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using King.Mapper;
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
        /// Blob Container
        /// </summary>
        private readonly IContainer container = null;

        /// <summary>
        /// Table
        /// </summary>
        private readonly ITableStorage table = null;

        /// <summary>
        /// Storage Queue
        /// </summary>
        private readonly IStorageQueue queue = null;
        #endregion

        #region Methods
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
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }
            if (null == elements)
            {
                throw new ArgumentNullException("elements");
            }

            this.container = new Container(elements.Container, connectionString);
            this.table = new TableStorage(elements.Table, connectionString);
            this.queue = new StorageQueue(elements.Queue, connectionString);
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
        public async Task Process(byte[] content, string contentType, string fileName)
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
            var extension = fileName.Substring(fileName.LastIndexOf('.'));
            var data = new RawData()
            {
                Contents = content,
                Identifier = id,
                ContentType = contentType,
                OriginalFileName = fileName,
                FileSize = content.LongLength,
                FileName = string.Format("{0}_{1}{2}", id, Original, extension),
            };

            var entity = data.Map<ImageEntity>();
            entity.PartitionKey = data.Identifier.ToString();
            entity.RowKey = Original;
            await table.InsertOrReplace(entity);

            var toQueue = new ImageQueued()
            {
                FileNameFormat = string.Format("{0}_{1}{2}", id, "{0}", extension),
            };

            await this.queue.Save(new CloudQueueMessage(JsonConvert.SerializeObject(toQueue)));

            await container.Save(data.FileName, data.Contents, data.ContentType);
        }
        #endregion
    }
}