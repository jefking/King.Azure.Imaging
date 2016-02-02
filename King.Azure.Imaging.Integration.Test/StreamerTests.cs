namespace King.Azure.Imaging.Integration.Test
{
    using King.Azure.Data;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [TestFixture]
    public class StreamerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";
        private IContainer container;

        [SetUp]
        public void Setup()
        {
            var name = 'a' + Guid.NewGuid().ToString();
            this.container = new Container(name, connectionString);
            container.CreateIfNotExists().Wait();
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
            var fileName = Guid.NewGuid().ToString();
            var contentType = "image/jpeg";

            await this.container.Save(fileName, bytes, contentType);

            var streamer = new Streamer(this.container);
            using (var stream = await streamer.Stream(fileName))
            {
                var ms = stream as MemoryStream;
                Assert.IsNotNull(ms);
                Assert.AreEqual(bytes, ms.ToArray());
            }

            Assert.AreEqual(contentType, streamer.MimeType);
        }
    }
}