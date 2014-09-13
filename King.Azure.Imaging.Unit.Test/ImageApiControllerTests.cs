namespace King.Azure.Imaging.Unit.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    [TestFixture]
    public class ImageApiControllerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new ImageApiController(connectionString);
        }

        [Test]
        public void IsApiController()
        {
            Assert.IsNotNull(new ImageApiController(connectionString) as ApiController);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            new ImageApiController(null, preprocessor, elements);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorImagePreprocessorNull()
        {
            var elements = Substitute.For<IStorageElements>();
            new ImageApiController(connectionString, null, elements);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorImagePreProcessorNull()
        {
            var streamer = Substitute.For<IImageStreamer>();
            new ImageApiController(null, streamer);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorImageStreamerNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            new ImageApiController(preprocessor, null);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorStorageElementsNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            new ImageApiController(connectionString, preprocessor, null);
        }

        [Test]
        public async Task GetFileNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());

            var api = new ImageApiController(connectionString, preprocessor, elements);
            var response = await api.Get(null);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
        }

        [Test]
        public async Task ResizeFileNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());

            var api = new ImageApiController(connectionString, preprocessor, elements);
            var response = await api.Resize(null, 100, 100);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
        }

        [Test]
        public async Task ResizeWidthInvalid()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());

            var api = new ImageApiController(connectionString, preprocessor, elements);
            var response =  await api.Resize(Guid.NewGuid().ToString(), -1, 100);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
        }

        [Test]
        public async Task ResizeHeightInvalid()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());

            var api = new ImageApiController(connectionString, preprocessor, elements);
            var response = await api.Resize(Guid.NewGuid().ToString(), 100, -1);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
        }

        [Test]
        public async Task WidthAndHeightZero()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());

            var api = new ImageApiController(connectionString, preprocessor, elements);
            var response = await api.Resize(Guid.NewGuid().ToString(), 0, 0);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
        }
    }
}