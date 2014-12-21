namespace King.Azure.Imaging.Tasks
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Image Dequeue Setup
    /// </summary>
    public class ImageDequeueSetup : QueueSetup<ImageQueued>
    {
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
            this.ConnectionString = config.ConnectionString;
            this.Name = config.StorageElements.Queue;
            this.Priority = config.Priority;
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
        #endregion

        #region Methods
        /// <summary>
        /// Get Processor
        /// </summary>
        /// <returns>Image Processor</returns>
        public override IProcessor<ImageQueued> Get()
        {
            return new Processor(new DataStore(this.ConnectionString), this.Images);
        }
        #endregion
    }
}