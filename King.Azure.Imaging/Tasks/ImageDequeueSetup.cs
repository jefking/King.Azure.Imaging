namespace King.Azure.Imaging.Tasks
{
    using System;
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service.Data;

    /// <summary>
    /// Image Dequeue Setup
    /// </summary>
    public class ImageDequeueSetup : QueueSetup<ImageQueued>
    {
        #region Members
        /// <summary>
        /// Connection String
        /// </summary>
        protected readonly string connectionString;
        #endregion

        #region Constructors
        /// <summary>
        /// Image Dequeue Setup
        /// </summary>
        /// <param name="config"></param>
        public ImageDequeueSetup(ITaskConfiguration config)
        {
            if (null == config)
            {
                throw new ArgumentNullException("config");
            }
            
            this.Images = config.Versions.Images;
            this.Name = config.StorageElements.Queue;
            this.Priority = config.Priority;
            this.connectionString = config.ConnectionString;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Image Versions to Generate
        /// </summary>
        public IReadOnlyDictionary<string, IImageVersion> Images
        {
            get;
            set;
        }

        public override Func<IProcessor<ImageQueued>> Processor
        {
            get
            {
                return () => { return new Processor(new DataStore(this.connectionString), this.Images); };
            }
        }
        #endregion
    }
}