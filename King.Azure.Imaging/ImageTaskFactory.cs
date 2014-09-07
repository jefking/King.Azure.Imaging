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
    public class ImageTaskFactory : ITaskFactory<object>
    {
        #region Methods
        /// <summary>
        /// Connection String
        /// </summary>
        private readonly string connectionString = null;

        /// <summary>
        /// Storage Elements
        /// </summary>
        private readonly IStorageElements elements = null;

        /// <summary>
        /// Versions
        /// </summary>
        private readonly IVersions versions = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageTaskFactory(string connectionString)
            : this(connectionString, new StorageElements(), new Versions())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImageTaskFactory(string connectionString, IStorageElements elements, IVersions versions)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }
            if (null == elements)
            {
                throw new ArgumentNullException("elements");
            }
            if (null == versions)
            {
                throw new ArgumentNullException("versions");
            }

            this.connectionString = connectionString;
            this.elements = elements;
            this.versions = versions;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough">passthrough</param>
        /// <returns>Runnable Tasks</returns>
        public IEnumerable<IRunnable> Tasks(object passthrough)
        {
            var tasks = new List<IRunnable>();

            //Blob Container
            var container = new Container(elements.Container, connectionString, true);
            var table = new TableStorage(elements.Table, connectionString);

            //Initialization Tasks
            tasks.Add(new InitializeStorageTask(container));
            tasks.Add(new InitializeStorageTask(table));
            tasks.Add(new InitializeStorageTask(new StorageQueue(elements.Queue, connectionString)));

            //Image Processing Task
            tasks.Add(new BackoffRunner(new StorageDequeue<ImageQueued>(elements.Queue, connectionString, new ImagingProcessor(container, table, this.versions.Images))));

            return tasks;
        }
        #endregion
    }
}