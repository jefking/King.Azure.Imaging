namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Imaging.Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;

    [TestFixture]
    public class ImageApiControllerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new ImageApi(connectionString);
        }

        [Test]
        public void IsApiController()
        {
            Assert.IsNotNull(new ImageApi(connectionString) as ApiController);
        }

        [Test]
        public void ConstructorConnectionStringNull()
        {
            var preprocessor = Substitute.For<IPreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            Assert.That(() => new ImageApi(null, preprocessor, elements), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorImagePreprocessorNull()
        {
            var elements = Substitute.For<IStorageElements>();
            Assert.That(() => new ImageApi(connectionString, null, elements), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorImagePreProcessorNull()
        {
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IDataStore>();
            Assert.That(() => new ImageApi(null, store), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorStoreNull()
        {
            var preprocessor = Substitute.For<IPreprocessor>();
            var imaging = Substitute.For<IImaging>();
            Assert.That(() => new ImageApi(preprocessor, null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorStorageElementsNull()
        {
            var preprocessor = Substitute.For<IPreprocessor>();
            Assert.That(() => new ImageApi(connectionString, preprocessor, null), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public async Task GetFileNull()
        {
            var preprocessor = Substitute.For<IPreprocessor>();
            var store = Substitute.For<IDataStore>();

            var api = new ImageApi(preprocessor, store);
            var response = await api.Get(null);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.PreconditionFailed, response.StatusCode);
        }

        [Test]
        public async Task Post()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var file = dir.Substring(6, dir.Length - 6) + @"\icon.png";
            var bytes = File.ReadAllBytes(file);
            var fileContent = new ByteArrayContent(bytes);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "myFilename.jpg"
            };

            var preProcessor = Substitute.For<IPreprocessor>();
            preProcessor.Process(bytes, "image/jpeg", "myFilename.jpg");

            var store = Substitute.For<IDataStore>();

            var api = new ImageApi(preProcessor, store)
            {
                Request = new HttpRequestMessage(),
            };
            var content = new MultipartContent();
            content.Add(fileContent);
            api.Request.Content = content;

            var response = await api.Post();

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Content);

            preProcessor.Received().Process(bytes, "image/jpeg", "myFilename.jpg");
        }

        [Test]
        public async Task PostMultiple()
        {
            var random = new Random();
            var count = random.Next(2, 5);
            var files = new List<string>(count);

            var preProcessor = Substitute.For<IPreprocessor>();

            var content = new MultipartContent();

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var f = dir.Substring(6, dir.Length - 6) + @"\icon.png";
            var bytes = File.ReadAllBytes(f);
            for (var i = 0; i < count; i++)
            {
                var fileName = Guid.NewGuid().ToString();
                var fileContent = new ByteArrayContent(bytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
                preProcessor.Process(bytes, "image/jpeg", fileName);

                content.Add(fileContent);
                files.Add(fileName);
            }

            var store = Substitute.For<IDataStore>();

            var api = new ImageApi(preProcessor, store)
            {
                Request = new HttpRequestMessage(),
            };
            api.Request.Content = content;

            var response = await api.Post();

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Content);

            foreach (var file in files)
            {
                preProcessor.Received().Process(bytes, "image/jpeg", file);
            }
        }

        [Test]
        public async Task PostInvalid()
        {
            var random = new Random();
            var bytes = new byte[128];
            random.NextBytes(bytes);

            var preProcessor = Substitute.For<IPreprocessor>();
            var store = Substitute.For<IDataStore>();

            var api = new ImageApi(preProcessor, store)
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
            var preProcessor = Substitute.For<IPreprocessor>();
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IDataStore>();

            var api = new ImageApi(preProcessor, store)
            {
                Request = new HttpRequestMessage(),
            };
            api.Request.Content = new MultipartContent();

            var response = await api.Post();

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}