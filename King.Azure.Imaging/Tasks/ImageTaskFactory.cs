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
    public class ImageTaskFactory : EasyTaskFactory<ITaskConfiguration>
    {
        #region Methods
        /// <summary>
        /// Load Tasks
        /// </summary>
        /// <param name="passthrough">passthrough</param>
        /// <returns>Runnable Tasks</returns>
        public override IEnumerable<IRunnable> Tasks(ITaskConfiguration config)
        {
            var elements = config.StorageElements;

            //Initialization Tasks
            yield return base.InitializeStorage(new Container(elements.Container, config.ConnectionString, true));
            yield return base.InitializeStorage(new TableStorage(elements.Table, config.ConnectionString));

            foreach (var t in new StorageDequeueFactory<ImageQueued>().Tasks(new ImageDequeueSetup(config)))
            {
                yield return t;
            }
        }
        #endregion
    }
}