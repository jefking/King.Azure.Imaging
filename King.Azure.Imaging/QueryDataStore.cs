namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Query Data Store
    /// </summary>
    public class QueryDataStore : IQueryDataStore
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
        public QueryDataStore(string connectionString)
            : this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Table Storage Connection String</param>
        /// <param name="elements">Storage Elements</param>
        public QueryDataStore(string connectionString, IStorageElements elements)
            : this(new TableStorage(elements.Table, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="table">Table</param>
        public QueryDataStore(ITableStorage table)
        {
            if (null == table)
            {
                throw new ArgumentNullException("table");
            }

            this.table = table;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Query Table Storage
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="version">Version</param>
        /// <param name="fileName">File Name</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<IDictionary<string, object>>> Query(Guid? identifier = null, string version = null, string fileName = null)
        {
            var partitionFilter = identifier.HasValue ? TableQuery.GenerateFilterCondition(TableStorage.PartitionKey, QueryComparisons.Equal, identifier.Value.ToString()) : null;
            var rowFilter = !string.IsNullOrWhiteSpace(version) ? TableQuery.GenerateFilterCondition(TableStorage.RowKey, QueryComparisons.Equal, version) : null;

            var query = new TableQuery();
            if (null != partitionFilter && null != rowFilter)
            {
                query.Where(TableQuery.CombineFilters(partitionFilter, TableOperators.And, rowFilter));
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

            if (null != images)
            {
                images = images.Where(i => string.IsNullOrWhiteSpace(fileName) || fileName == (string)i["FileName"]);
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
            }

            return images;
        }
        #endregion
    }
}