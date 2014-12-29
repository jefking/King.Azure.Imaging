namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Linq;

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
        public virtual async Task<HttpResponseMessage> Get(Guid? id = null, string version = null, string file = null)
        {
            var partitionFilter = id.HasValue ? TableQuery.GenerateFilterCondition(TableStorage.PartitionKey, QueryComparisons.Equal, id.Value.ToString()) : null;
            var rowFilter = !string.IsNullOrWhiteSpace(version) ? TableQuery.GenerateFilterCondition(TableStorage.RowKey, QueryComparisons.Equal, version) : null;

            var query = new TableQuery();
            if (null != partitionFilter && null != rowFilter)
            {
                query = query.Where(TableQuery.CombineFilters(partitionFilter, TableOperators.And, rowFilter));
            }
            else if (null != partitionFilter)
            {
                query.Where(partitionFilter);
            }
            else if (null != rowFilter)
            {
                query.Where(rowFilter);
            }

            var images = await this.table.Query(query);
            images = images.Where(i => string.IsNullOrWhiteSpace(file) || file == (string)i["FileName"]);
            foreach (var data in images)
            {
                data.Add("Identifier", data[TableStorage.PartitionKey]);
                data.Add("Version", data[TableStorage.RowKey]);
                data.Add("CreatedOn", data[TableStorage.Timestamp]);

                data.Remove(TableStorage.PartitionKey);
                data.Remove(TableStorage.Timestamp);
                data.Remove(TableStorage.RowKey);
                data.Remove(TableStorage.ETag);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(images), Encoding.UTF8, "application/json"),
            };
        }
        #endregion
    }
}