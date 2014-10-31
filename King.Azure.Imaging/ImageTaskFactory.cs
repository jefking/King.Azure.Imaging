namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service;
    using King.Service.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Image Task Factory
    /// </summary>
    public class ImageTaskFactory : ITaskFactory<IStorageElements>
    {
        #region Methods
        /// <summary>
        /// Connection String
        /// </summary>
        protected readonly string connectionString = null;

        /// <summary>
        /// Versions
        /// </summary>
        protected readonly IVersions versions = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageTaskFactory(string connectionString)
            : this(connectionString, new Versions())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageTaskFactory(string connectionString, IVersions versions)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }
            if (null == versions)
            {
                throw new ArgumentNullException("versions");
            }

            this.connectionString = connectionString;
            this.versions = versions;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough">passthrough</param>
        /// <returns>Runnable Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(IStorageElements elements)
        {
            //Storage
            var container = new Container(elements.Container, connectionString, true);
            var table = new TableStorage(elements.Table, connectionString);
            var queue = new StorageQueue(elements.Queue, connectionString);

            //Initialization Tasks
            yield return new InitializeStorageTask(container);
            yield return new InitializeStorageTask(table);
            yield return new InitializeStorageTask(queue);

            //Queue Poller
            var poller = new StorageQueuePoller<ImageQueued>(queue);
            //Image Processor
            var processor = new Processor(new DataStore(connectionString), this.versions.Images);
            //Image Processing Task
            yield return new BackoffRunner(new DequeueBatch<ImageQueued>(poller, processor));
        }
        #endregion
    }
}