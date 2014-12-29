namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using King.Mapper;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Image Meta Data Controller
    /// </summary>
    public class ImageDataApiController : ApiController
    {
        #region Members
        /// <summary>
        /// Table Storage (Image Meta-Data)
        /// </summary>
        protected readonly ITableStorage table = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Table Storage Connection String</param>
        public ImageDataApiController(string connectionString)
            : this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Table Storage Connection String</param>
        /// <param name="elements">Storage Elements</param>
        public ImageDataApiController(string connectionString, IStorageElements elements)
            :this(new TableStorage(elements.Table, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="table">Table</param>
        public ImageDataApiController(ITableStorage table)
        {
            this.table = table;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get image data
        /// </summary>
        /// <returns>Image Data</returns>
        public virtual async Task<HttpResponseMessage> Get(Guid? id = null, string fileName = null)
        {
            var query = new TableQuery();
            var images = await this.table.Query(query);
            foreach (var data in images)
            {
                data.Remove(TableStorage.ETag);

                var temp = data[TableStorage.PartitionKey];
                data.Remove(TableStorage.PartitionKey);
                data.Add("Identifier", temp);

                temp = data[TableStorage.RowKey];
                data.Remove(TableStorage.RowKey);
                data.Add("Version", temp);

                temp = data[TableStorage.Timestamp];
                data.Remove(TableStorage.Timestamp);
                data.Add("CreatedOn", temp);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(images), Encoding.UTF8, "application/json"),
            };
        }
        #endregion
    }
}