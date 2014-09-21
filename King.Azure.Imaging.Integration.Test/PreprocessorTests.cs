namespace King.Azure.Imaging.Integration.Test
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class PreprocessorTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";
        private IContainer container;
        private ITableStorage table;
        private IStorageQueue queue;

        [SetUp]
        public void Setup()
        {
            var elements = new StorageElements();
            this.container = new Container(elements.Container, connectionString);
            this.container.CreateIfNotExists().Wait();
            this.table = new TableStorage(elements.Table, connectionString);
            this.table.CreateIfNotExists().Wait();
            this.queue = new StorageQueue(elements.Queue, connectionString);
            this.queue.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            this.container.Delete().Wait();
            this.table.Delete().Wait();
            this.queue.Delete().Wait();
        }

        [Test]
        public async Task Process()
        {
            var bytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\icon.png");
            var fileName = Guid.NewGuid().ToString();
            var originalFileName = string.Format(Naming.FileNameFormat, fileName, Naming.Original, Naming.DefaultExtension);
            var contentType = "image/jpeg";

            var preProcessor = new Preprocessor(connectionString);
            await preProcessor.Process(bytes, contentType, fileName);

            //Table
            var entity = (from e in this.table.QueryByRow<ImageEntity>(Naming.Original)
                          select e).FirstOrDefault();

            Assert.IsNotNull(entity);
            Assert.AreEqual(contentType, entity.MimeType);
            Assert.AreEqual(string.Format(Naming.PathFormat, this.container.Name, entity.FileName), entity.RelativePath);
            Assert.AreEqual(bytes.LongLength, entity.FileSize);

            //Container
            var data = await this.container.Get(entity.FileName);
            Assert.AreEqual(bytes, data);

            //Queue
            var queued = await this.queue.Get();
            Assert.IsNotNull(queued);

            var imgQueued = JsonConvert.DeserializeObject<ImageQueued>(queued.AsString);
            Assert.IsNotNull(imgQueued);
            Assert.AreEqual(Guid.Parse(entity.PartitionKey), imgQueued.Identifier);
            Assert.AreEqual(string.Format(Naming.FileNameFormat, entity.PartitionKey, "{0}", "{1}"), imgQueued.FileNameFormat);
            Assert.AreEqual(Naming.DefaultExtension, imgQueued.OriginalExtension);
        }
    }
}