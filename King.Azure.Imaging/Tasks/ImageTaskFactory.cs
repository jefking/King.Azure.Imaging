namespace King.Azure.Imaging.Tasks
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Azure.Imaging.Tasks;
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

            //Storage
            var container = new Container(elements.Container, config.ConnectionString, true);
            var table = new TableStorage(elements.Table, config.ConnectionString);
            var queue = new StorageQueue(elements.Queue, config.ConnectionString);

            //Initialization Tasks
            yield return new InitializeStorageTask(container);
            yield return new InitializeStorageTask(table);
            yield return new InitializeStorageTask(queue);

            //Dequeue, Image Processor (dynamic scale)
            yield return new DequeueScaler(config);
        }
        #endregion
    }
}