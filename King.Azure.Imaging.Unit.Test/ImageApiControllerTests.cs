namespace King.Azure.Imaging.Unit.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
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
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IImageStore>();
            new ImageApiController(null, streamer, imaging, store);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorImageStreamerNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var store = Substitute.For<IImageStore>();
            var imaging = Substitute.For<IImaging>();
            new ImageApiController(preprocessor, null, imaging, store);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorImagingNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var streamer = Substitute.For<IImageStreamer>();
            var store = Substitute.For<IImageStore>();
            new ImageApiController(preprocessor, streamer, null, store);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorStoreNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var streamer = Substitute.For<IImageStreamer>();
            var imaging = Substitute.For<IImaging>();
            new ImageApiController(preprocessor, streamer, imaging, null);
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

        [Test]
        public async Task Post()
        {
            var bytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\icon.png");
            var fileContent = new ByteArrayContent(bytes);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "myFilename.jpg"
            };

            var preProcessor = Substitute.For<IImagePreprocessor>();
            preProcessor.Process(bytes, "image/jpeg", "myFilename.jpg");

            var streamer = Substitute.For<IImageStreamer>();
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IImageStore>();

            var api = new ImageApiController(preProcessor, streamer, imaging, store)
            {
                Request = new HttpRequestMessage(),
            };
            var content = new MultipartContent();
            content.Add(fileContent);
            api.Request.Content = content;

            var response = await api.Post();

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            preProcessor.Received().Process(bytes, "image/jpeg", "myFilename.jpg");
        }

        [Test]
        public async Task PostInvalid()
        {
            var random = new Random();
            var bytes = new byte[128];
            random.NextBytes(bytes);

            var preProcessor = Substitute.For<IImagePreprocessor>();
            var streamer = Substitute.For<IImageStreamer>();
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IImageStore>();

            var api = new ImageApiController(preProcessor, streamer, imaging, store)
            {
                Request = new HttpRequestMessage(),
            };
            api.Request.Content = new ByteArrayContent(bytes);

            var response = await api.Post();

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }

        [Test]
        public async Task PostEmpty()
        {
            var preProcessor = Substitute.For<IImagePreprocessor>();
            var streamer = Substitute.For<IImageStreamer>();
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IImageStore>();

            var api = new ImageApiController(preProcessor, streamer, imaging, store)
            {
                Request = new HttpRequestMessage(),
            };
            api.Request.Content = new MultipartContent();

            var response = await api.Post();

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}