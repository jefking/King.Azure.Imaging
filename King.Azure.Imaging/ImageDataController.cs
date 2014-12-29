namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using King.Mapper;
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
    public class ImageDataController : ApiController
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
        public ImageDataController(string connectionString)
            : this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Table Storage Connection String</param>
        /// <param name="elements">Storage Elements</param>
        public ImageDataController(string connectionString, IStorageElements elements)
            :this(new TableStorage(elements.Table, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="table">Table</param>
        public ImageDataController(ITableStorage table)
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
            var images = await this.table.QueryByPartition<ImageEntity>(id.ToString());
            var data = images.Select(i => i.ToDictionary());
            foreach (var d in data)
            {
                d.Remove(TableStorage.ETag);
                
                var temp = d[TableStorage.PartitionKey];
                d.Remove(TableStorage.PartitionKey);
                d.Add("Identifier", temp);
                
                temp = d[TableStorage.RowKey];
                d.Remove(TableStorage.RowKey);
                d.Add("Version", temp);

                temp = d[TableStorage.Timestamp];
                d.Remove(TableStorage.Timestamp);
                d.Add("CreatedOn", temp);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"),
            };
        }
        #endregion
    }
}