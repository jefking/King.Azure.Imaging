namespace King.Azure.Imaging.Integration.Test
{
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImageApiControllerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";
        private IContainer container;

        [SetUp]
        public void Setup()
        {
            var name = 'a' + Guid.NewGuid().ToString().Replace('-', 'a').ToLowerInvariant();
            this.container = new Container(name, connectionString);
            this.container.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            this.container.Delete().Wait();
        }

        [Test]
        public async Task Get()
        {
            var random = new Random();
            var bytes = new byte[128];
            random.NextBytes(bytes);

            var file = Guid.NewGuid().ToString();

            await this.container.Save(file, bytes, "image/jpeg");

            var api = new ImageApiController(connectionString);
            var data = await api.Get(file);

            Assert.IsNotNull(data);
            Assert.AreEqual(bytes, await data.Content.ReadAsByteArrayAsync());
            Assert.AreEqual("image/jpeg", data.Content.Headers.ContentType.MediaType);
        }

        [Test]
        public async Task Resize()
        {
            var bytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\icon.png");

            var file = Guid.NewGuid().ToString();

            await this.container.Save(file, bytes, "image/png");

            var api = new ImageApiController(connectionString);
            var data = await api.Resize(file, 10);

            Assert.IsNotNull(data);
            var resized = await data.Content.ReadAsByteArrayAsync();
            Assert.IsTrue(bytes.LongLength > resized.LongLength);
            Assert.AreEqual("image/jpeg", data.Content.Headers.ContentType.MediaType);
        }
    }
}