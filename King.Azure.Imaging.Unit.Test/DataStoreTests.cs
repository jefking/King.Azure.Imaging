namespace King.Azure.Imaging.Unit.Test
{
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    [TestFixture]
    public class DataStoreTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new DataStore(connectionString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImagingNull()
        {
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            new DataStore(null, container, table, queue, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            var imaging = Substitute.For<IImaging>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            new DataStore(imaging, null, table, queue, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            new DataStore(imaging, container, null, queue, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var naming = Substitute.For<INaming>();
            new DataStore(imaging, container, table, null, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImageNamingNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            new DataStore(imaging, container, table, queue, null);
        }

        [Test]
        public void Streamer()
        {
            var store = new DataStore(connectionString);
            Assert.IsNotNull(store.Streamer);
        }

        [Test]
        public async Task Save()
        {
            var random = new Random();
            var fileName = Guid.NewGuid().ToString();
            var content = new byte[32];
            random.NextBytes(content);
            var version = Guid.NewGuid().ToString();
            var mimeType = Guid.NewGuid().ToString();
            var identifier = Guid.NewGuid();
            var queueForResize = false;
            var extension = Guid.NewGuid().ToString();
            var quality = 100;
            var width = 122;
            var height = 133;

            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            container.Save(fileName, content, mimeType);
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ImageEntity>());
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();

            var store = new DataStore(imaging, container, table, queue, naming);
            await store.Save(fileName, content, version, mimeType, identifier, queueForResize, extension, quality, width, height);

            queue.Received(0).Save(Arg.Any<ImageQueued>());
            container.Received().Save(fileName, content, mimeType);
            table.Received().InsertOrReplace(Arg.Any<ImageEntity>());
        }

        [Test]
        public async Task SaveUnknownSize()
        {
            var random = new Random();
            var fileName = Guid.NewGuid().ToString();
            var content = new byte[32];
            random.NextBytes(content);
            var version = Guid.NewGuid().ToString();
            var mimeType = Guid.NewGuid().ToString();
            var identifier = Guid.NewGuid();
            var queueForResize = false;
            var extension = Guid.NewGuid().ToString();
            var quality = 100;
            var width = 0;
            var height = 0;
            var size = new Size()
            {
                Height = 99,
                Width = 1000,
            };

            var imaging = Substitute.For<IImaging>();
            imaging.Size(content).Returns(size);
            var container = Substitute.For<IContainer>();
            container.Save(fileName, content, mimeType);
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ImageEntity>());
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();

            var store = new DataStore(imaging, container, table, queue, naming);
            await store.Save(fileName, content, version, mimeType, identifier, queueForResize, extension, quality, width, height);

            imaging.Received().Size(content);
            container.Received().Save(fileName, content, mimeType);
            table.Received().InsertOrReplace(Arg.Any<ImageEntity>());
        }

        [Test]
        public async Task SaveQueue()
        {
            var random = new Random();
            var fileName = Guid.NewGuid().ToString();
            var content = new byte[32];
            random.NextBytes(content);
            var version = Guid.NewGuid().ToString();
            var mimeType = Guid.NewGuid().ToString();
            var identifier = Guid.NewGuid();
            var queueForResize = true;
            var extension = Guid.NewGuid().ToString();
            var quality = 100;
            var width = 122;
            var height = 133;

            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            container.Save(fileName, content, mimeType);
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ImageEntity>());
            var queue = Substitute.For<IStorageQueue>();
            queue.Save(Arg.Any<ImageQueued>());
            var naming = Substitute.For<INaming>();

            var store = new DataStore(imaging, container, table, queue, naming);
            await store.Save(fileName, content, version, mimeType, identifier, queueForResize, extension, quality, width, height);

            queue.Received().Save(Arg.Any<ImageQueued>());
            container.Received().Save(fileName, content, mimeType);
            table.Received().InsertOrReplace(Arg.Any<ImageEntity>());
        }

        [Test]
        public async Task Resize()
        {
            var fileName = Guid.NewGuid().ToString();
            var width = 122;
            var format = new JpegFormat();
            var id = Guid.NewGuid();
            var versionName = Guid.NewGuid().ToString();
            var cachedFileName = Guid.NewGuid().ToString();

            var imaging = Substitute.For<IImaging>();
            imaging.Get(Naming.DefaultExtension, 85).Returns(format);
            imaging.Resize(Arg.Any<byte[]>(), Arg.Any<ImageVersion>());
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            naming.FromFileName(fileName).Returns(id);
            naming.DynamicVersion(format.DefaultExtension, 85, width, 0).Returns(versionName);
            naming.FileName(id, versionName, format.DefaultExtension).Returns(cachedFileName);

            var store = new DataStore(imaging, container, table, queue, naming);
            var result = await store.Resize(fileName, width);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Raw);
            Assert.AreEqual(format.MimeType, result.MimeType);

            naming.Received().FromFileName(fileName);
            naming.Received().DynamicVersion(format.DefaultExtension, 85, width, 0);
            naming.Received().FileName(id, versionName, format.DefaultExtension);
            imaging.Received().Get(Naming.DefaultExtension, 85);
            imaging.Received().Resize(Arg.Any<byte[]>(), Arg.Any<ImageVersion>());
        }

        [Test]
        public async Task ResizeCache()
        {
            var fileName = Guid.NewGuid().ToString();
            var width = 122;
            var format = new JpegFormat();
            var id = Guid.NewGuid();
            var versionName = Guid.NewGuid().ToString();
            var cachedFileName = Guid.NewGuid().ToString();

            var imaging = Substitute.For<IImaging>();
            imaging.Get(Naming.DefaultExtension, 85).Returns(format);
            imaging.Resize(Arg.Any<byte[]>(), Arg.Any<ImageVersion>());
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            naming.FromFileName(fileName).Returns(id);
            naming.DynamicVersion(format.DefaultExtension, 85, width, 0).Returns(versionName);
            naming.FileName(id, versionName, format.DefaultExtension).Returns(cachedFileName);

            var store = new DataStore(imaging, container, table, queue, naming);
            var result = await store.Resize(fileName, width, 0, Naming.DefaultExtension, 85, true);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Raw);
            Assert.AreEqual(format.MimeType, result.MimeType);

            naming.Received().FromFileName(fileName);
            naming.Received().DynamicVersion(format.DefaultExtension, 85, width, 0);
            naming.Received().FileName(id, versionName, format.DefaultExtension);
            imaging.Received().Get(Naming.DefaultExtension, 85);
            imaging.Received().Resize(Arg.Any<byte[]>(), Arg.Any<ImageVersion>());
        }
    }
}