namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using King.Mapper;

    public class ImageController : ApiController
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
        private readonly Container container = new Container("name", "connection string");

        /// <summary>
        /// Table
        /// </summary>
        private readonly TableStorage table = new TableStorage("name", "connection string");
        #endregion

        public async Task UploadImage()
        {
            var data = new RawData()
            {
                Contents = await Request.Content.ReadAsByteArrayAsync(),
                Identifier = Guid.NewGuid(),
                ContentType = Request.Headers.GetValues(ContentTypeHeader).FirstOrDefault(),
                FileName = Request.Headers.GetValues(FileNameHeader).FirstOrDefault(),
            };
            data.FileSize = data.Contents.Length;

            var entity = data.Map<ImageEntity>();
            entity.PartitionKey = "original";
            entity.RowKey = data.Identifier.ToString();
            await table.InsertOrReplace(entity);

            //Queue for additional processing.

            await container.Save(data.Identifier.ToString(), data.Contents, data.ContentType);
        }
    }
}