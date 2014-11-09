namespace King.Azure.Imaging.Tasks
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service;
    using King.Service.Data;
    using System.Collections.Generic;

    public class DequeueScaler : AutoScaler<ITaskConfiguration>
    {
        public DequeueScaler(ITaskConfiguration configuration)
            : base(configuration)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(ITaskConfiguration config)
        {
            var elements = config.StorageElements;
            var queue = new StorageQueue(elements.Queue, config.ConnectionString);

            //Queue Poller
            var poller = new StorageQueuePoller<ImageQueued>(queue);
            //Image Processor
            var processor = new Processor(new DataStore(config.ConnectionString), config.Versions.Images);
            //Image Processing Task
            yield return new BackoffRunner(new DequeueBatch<ImageQueued>(poller, processor));
        }
    }
}