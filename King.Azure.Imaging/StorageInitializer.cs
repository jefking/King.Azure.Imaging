namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Storage Initializer
    /// </summary>
    public class StorageInitializer
    {
        #region Members
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
        public StorageInitializer(string connectionString)
            :this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public StorageInitializer(string connectionString, IStorageElements elements)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {

            }
            if (null == elements)
            {

            }

            this.container = new Container(elements.Container, connectionString);
            this.table = new TableStorage(elements.Table, connectionString);
            this.queue = new StorageQueue(elements.Queue, connectionString);
        }
        #endregion

        public async Task Init()
        {
            await this.container.CreateIfNotExists();
            await this.table.CreateIfNotExists();
            await this.queue.CreateIfNotExists();
        }
    }
}