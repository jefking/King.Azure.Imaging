namespace King.Azure.Imaging.Tasks
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;

    /// <summary>
    /// Image Task Factory
    /// </summary>
    public class ImageTaskFactory : ITaskFactory<ITaskConfiguration>
    {
        #region Methods
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough">passthrough</param>
        /// <returns>Runnable Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(ITaskConfiguration config)
        {
            var elements = config.StorageElements;

            //Initialization Tasks
            yield return new InitializeStorageTask(new Container(elements.Container, config.ConnectionString, true));
            yield return new InitializeStorageTask(new TableStorage(elements.Table, config.ConnectionString));

            foreach (var t in new StorageDequeueFactory<ImageQueued>().Tasks(new ImageDequeueSetup(config)))
            {
                yield return t;
            }
        }
        #endregion
    }
}