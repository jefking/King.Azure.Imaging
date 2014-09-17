namespace King.Azure.Imaging.Integration.Test
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImagePreprocessorTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";
        private IContainer container;
        private ITableStorage table;
        private IStorageQueue queue;

        [SetUp]
        public void Setup()
        {
            var name = 'a' + Guid.NewGuid().ToString().Replace('-', 'a').ToLowerInvariant();
            this.container = new Container(name, connectionString);
            this.container.CreateIfNotExists().Wait();
            this.table = new TableStorage(name, connectionString);
            this.table.CreateIfNotExists().Wait();
            this.queue = new StorageQueue(name, connectionString);
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
        [Ignore]
        public async Task Process()
        {
            var random = new Random();
            var bytes = new byte[128];
            random.NextBytes(bytes);
            var fileName = Guid.NewGuid().ToString();
            var originalFileName = string.Format(ImageNaming.FileNameFormat, fileName, ImageNaming.Original, ImageNaming.DefaultExtension);
            var contentType = "image/jpeg";
            var size = new Size()
            {
                Width = random.Next(),
                Height = random.Next(),
            };

            var imaging = Substitute.For<IImaging>();
            imaging.Size(bytes).Returns(size);

            //var preProcessor = new ImagePreprocessor(imaging, this.container, this.table, this.queue);
            //await preProcessor.Process(bytes, contentType, fileName);

            //var entity = (from e in this.table.QueryByRow<ImageEntity>(ImageNaming.Original)
            //              select e).FirstOrDefault();

            //Assert.IsNotNull(entity);
            //Assert.AreEqual(contentType, entity.ContentType);
            //Assert.AreEqual(string.Format(ImageNaming.PathFormat, this.container.Name, entity.FileName), entity.RelativePath);
            //Assert.AreEqual(bytes.LongLength, entity.FileSize);
            //Assert.AreEqual(size.Width, entity.Width);
            //Assert.AreEqual(size.Height, entity.Height);

            //var data = await this.container.Get(entity.FileName);
            //Assert.AreEqual(bytes, data);

            //var queued = await this.queue.Get();
            //Assert.IsNotNull(queued);

            //var imgQueued = JsonConvert.DeserializeObject<ImageQueued>(queued.AsString);
            //Assert.IsNotNull(imgQueued);
            //Assert.AreEqual(Guid.Parse(entity.PartitionKey), imgQueued.Identifier);
            //Assert.AreEqual(string.Format(ImagePreprocessor.FileNameFormat, entity.PartitionKey, "{0}", "{1}"), imgQueued.FileNameFormat);
            //Assert.AreEqual(ImagePreprocessor.DefaultExtension, imgQueued.OriginalExtension);

            //imaging.Received().Size(bytes);
        }
    }
}