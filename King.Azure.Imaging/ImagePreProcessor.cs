namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using King.Mapper;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Preprocessor
    /// </summary>
    public class ImagePreprocessor : IImagePreprocessor
    {
        #region Members
        /// <summary>
        /// File Name Header
        /// </summary>
        public const string FileNameHeader = "X-File-Name";

        /// <summary>
        /// Content Type Header
        /// </summary>
        public const string ContentTypeHeader = "X-File-Type";

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
            var id = Guid.NewGuid();
            var extension = fileName.Substring(fileName.LastIndexOf('.'));
            var data = new RawData()
            {
                Contents = content,
                Identifier = id,
                ContentType = contentType,
                OriginalFileName = fileName,
                FileSize = content.Length,
                FileName = string.Format("{0}_original{1}", id, extension),
            };

            var entity = data.Map<ImageEntity>();
            entity.PartitionKey = "original";
            entity.RowKey = data.Identifier.ToString();
            await table.InsertOrReplace(entity);

            var toQueue = data.Map<ImageQueued>();
            await this.queue.Save(new CloudQueueMessage(JsonConvert.SerializeObject(toQueue)));

            await container.Save(data.FileName, data.Contents, data.ContentType);
        }
        #endregion
    }
}