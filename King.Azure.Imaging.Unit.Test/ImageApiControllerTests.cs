namespace King.Azure.Imaging.Unit.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
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
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFileNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            var api = new ImageApiController(connectionString, preprocessor, elements);
            await api.Get(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ResizeFileNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            var api = new ImageApiController(connectionString, preprocessor, elements);
            await api.Resize(null, 100, 100);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ResizeWidthInvalid()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            var api = new ImageApiController(connectionString, preprocessor, elements);
            await api.Resize(Guid.NewGuid().ToString(), 0, 100);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ResizeHeightInvalid()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            var api = new ImageApiController(connectionString, preprocessor, elements);
            await api.Resize(Guid.NewGuid().ToString(), 100, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ResizeFormatNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            var api = new ImageApiController(connectionString, preprocessor, elements);
            await api.Resize(Guid.NewGuid().ToString(), 100, 100, null);
        }
    }
}