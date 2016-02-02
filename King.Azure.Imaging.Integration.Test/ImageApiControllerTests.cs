namespace King.Azure.Imaging.Integration.Test
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImageApiControllerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";
        private IContainer container;
        private ITableStorage table;

        [SetUp]
        public void Setup()
        {
            var elements = new StorageElements();
            this.container = new Container(elements.Container, connectionString);
            this.container.CreateIfNotExists().Wait();
            this.table = new TableStorage(elements.Table, connectionString);
            this.table.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            this.container.Delete().Wait();
            this.table.Delete().Wait();
        }

        [Test]
        public async Task Get()
        {
            var random = new Random();
            var bytes = new byte[128];
            random.NextBytes(bytes);

            var file = Guid.NewGuid().ToString();

            await this.container.Save(file, bytes, "image/jpeg");

            var api = new ImageApi(connectionString);
            var data = await api.Get(file);

            Assert.IsNotNull(data);
            Assert.AreEqual(bytes, await data.Content.ReadAsByteArrayAsync());
            Assert.AreEqual("image/jpeg", data.Content.Headers.ContentType.MediaType);
        }

        [Test]
        public async Task Resize()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var f = dir.Substring(6, dir.Length - 6) + @"\icon.png";
            var bytes = File.ReadAllBytes(f);

            var file = Guid.NewGuid().ToString() + "_.png";

            await this.container.Save(file, bytes, "image/png");

            var api = new ImageApi(connectionString);
            var data = await api.Get(file, 10);

            Assert.IsNotNull(data);
            var resized = await data.Content.ReadAsByteArrayAsync();
            Assert.IsTrue(bytes.LongLength > resized.LongLength);
            Assert.AreEqual("image/jpeg", data.Content.Headers.ContentType.MediaType);
        }
    }
}