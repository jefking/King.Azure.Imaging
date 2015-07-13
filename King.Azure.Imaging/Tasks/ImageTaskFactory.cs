namespace King.Azure.Imaging.Tasks
{
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service;
    using King.Service.Data;

    /// <summary>
    /// Image Task Factory
    /// </summary>
    public class ImageTaskFactory : ITaskFactory<ITaskConfiguration>
    {
        #region Methods
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="config">Task Configuration</param>
        /// <returns>Runnable Tasks</returns>
        public virtual IEnumerable<IRunnable> Tasks(ITaskConfiguration config)
        {
            var elements = config.StorageElements;

            //Initialization Tasks
            yield return new InitializeStorageTask(new Container(elements.Container, config.ConnectionString, true));
            yield return new InitializeStorageTask(new TableStorage(elements.Table, config.ConnectionString));

            var factory = new DequeueFactory(config.ConnectionString);
            foreach (var t in factory.Tasks<ImageQueued>(new ImageDequeueSetup(config)))
            {
                yield return t;
            }
        }
        #endregion
    }
}