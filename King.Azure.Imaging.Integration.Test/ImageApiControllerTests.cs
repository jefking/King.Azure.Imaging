namespace King.Azure.Imaging.Integration.Test
{
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
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
        public async Task Post()
        {
            var preProcessor = Substitute.For<IImagePreprocessor>();
            var streamer = Substitute.For<IImageStreamer>();
            var imaging = Substitute.For<IImaging>();

            var api = new ImageApiController(preProcessor, streamer, imaging)
            {
                Request = new HttpRequestMessage(),
            };
            api.Request.Content = new MultipartContent();

            var response = await api.Post();

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Get()
        {
            var random = new Random();
            var bytes = new byte[128];
            random.NextBytes(bytes);

            var file = Guid.NewGuid().ToString();

            await this.container.Save(file, bytes, "image/jpeg");

            var preProcessor = Substitute.For<IImagePreprocessor>();
            var streamer = new ImageStreamer(this.container);
            var imaging = Substitute.For<IImaging>();

            var api = new ImageApiController(preProcessor, streamer, imaging);
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

            var preProcessor = Substitute.For<IImagePreprocessor>();
            var streamer = new ImageStreamer(this.container);
            var imaging = Substitute.For<IImaging>();

            var api = new ImageApiController(preProcessor, streamer, imaging);
            var data = await api.Resize(file, 10);

            Assert.IsNotNull(data);
            var resized = await data.Content.ReadAsByteArrayAsync();
            Assert.IsTrue(bytes.LongLength > resized.LongLength);
            Assert.AreEqual("image/jpeg", data.Content.Headers.ContentType.MediaType);
        }
    }
}