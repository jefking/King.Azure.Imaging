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
        private readonly string connectionString = null;
        private readonly IStorageElements elements = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public ImageTaskFactory(string connectionString)
            :this(connectionString, new StorageElements())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="elements"></param>
        public ImageTaskFactory(string connectionString, IStorageElements elements)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }
            if (null == elements)
            {
                throw new ArgumentNullException("elements");
            }

            this.connectionString = connectionString;
            this.elements = elements;
        }
        #endregion

        #region Methods
        public IEnumerable<IRunnable> Tasks(object passthrough)
        {
            var tasks = new List<IRunnable>();

            //Blob Container
            var container = new Container(elements.Container, connectionString);

            //Initialization Tasks
            tasks.Add(new InitializeStorageTask(container));
            tasks.Add(new InitializeStorageTask(new TableStorage(elements.Table, connectionString)));
            tasks.Add(new InitializeStorageTask(new StorageQueue(elements.Queue, connectionString)));

            //Dequeuing Task
            tasks.Add(new BackoffRunner(new StorageDequeue<ImageQueued>(elements.Queue, connectionString, new ImagingProcessor(container, this.Versions()))));

            return tasks;
        }

        public IDictionary<string, string> Versions()
        {
            var versions = new Dictionary<string, string>();
            
            //Define the versions to generate
            versions.Add("Thumb", "width=100&height=100&crop=auto&format=jpg");
            versions.Add("Medium", "maxwidth=400&maxheight=400&format=jpg");
            versions.Add("Large", "maxwidth=1900&maxheight=1900&format=jpg");
            
            return versions;
        }
        #endregion
    }
}