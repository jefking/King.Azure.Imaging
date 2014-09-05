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
        {
            this.container = new Container(StorageElements.Container, connectionString);
            this.table = new TableStorage(StorageElements.Table, connectionString);
            this.queue = new StorageQueue(StorageElements.Queue, connectionString);
        }
        #endregion

        #region Methods
        public async Task Process(byte[] content, string contentType, string fileName)
        {
            var data = new RawData()
            {
                Contents = content,
                Identifier = Guid.NewGuid(),
                ContentType = contentType,
                FileName = fileName,
            };
            data.FileSize = data.Contents.Length;

            var entity = data.Map<ImageEntity>();
            entity.PartitionKey = "original";
            entity.RowKey = data.Identifier.ToString();
            await table.InsertOrReplace(entity);

            var toQueue = data.Map<ImageQueued>();
            await this.queue.Save(new CloudQueueMessage(JsonConvert.SerializeObject(toQueue)));

            await container.Save(data.Identifier.ToString(), data.Contents, data.ContentType);
        }
        #endregion
    }
}