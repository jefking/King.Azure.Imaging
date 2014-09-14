namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using Microsoft.WindowsAzure.Storage.Queue;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImagePreprocessorTests
    {
        [Test]
        public void Constructor()
        {
            var connectionString = "UseDevelopmentStorage=true";
            new ImagePreprocessor(connectionString);
        }

        [Test]
        public void IsIImagePreprocessor()
        {
            var connectionString = "UseDevelopmentStorage=true";
            Assert.IsNotNull(new ImagePreprocessor(connectionString) as IImagePreprocessor);
        }

        [Test]
        public void Original()
        {
            Assert.AreEqual("original", ImagePreprocessor.Original);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new ImagePreprocessor(null);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ConstructorElementsNull()
        {
            var connectionString = "UseDevelopmentStorage=true";
            new ImagePreprocessor(connectionString, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImagingNull()
        {
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            new ImagePreprocessor(null, container, table, queue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            var imaging = Substitute.For<IImaging>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            new ImagePreprocessor(imaging, null, table, queue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var queue = Substitute.For<IStorageQueue>();
            new ImagePreprocessor(imaging, container, null, queue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            new ImagePreprocessor(imaging, container, table, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ProcessContentNull()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());

            var ip = new ImagePreprocessor(connectionString, elements);
            await ip.Process(null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ProcessContentTypeNull()
        {
            var random = new Random();
            var bytes = new byte[64];
            random.NextBytes(bytes);
            var connectionString = "UseDevelopmentStorage=true";
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());

            var ip = new ImagePreprocessor(connectionString, elements);
            await ip.Process(bytes, null, Guid.NewGuid().ToString());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ProcessFileNameNull()
        {
            var random = new Random();
            var bytes = new byte[64];
            random.NextBytes(bytes);
            var connectionString = "UseDevelopmentStorage=true";
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());

            var ip = new ImagePreprocessor(connectionString, elements);
            await ip.Process(bytes, Guid.NewGuid().ToString(), null);
        }

        private static readonly byte[] image = File.ReadAllBytes(Environment.CurrentDirectory + "\\icon.png");

        [Test]
        public async Task Process()
        {
            var bytes = image;
            var contentType = Guid.NewGuid().ToString();
            var fileName = string.Format("{0}.jpg", Guid.NewGuid());
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            container.Save(Arg.Any<string>(), bytes, contentType);
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ImageEntity>());
            var queue = Substitute.For<IStorageQueue>();
            queue.Save(Arg.Any<CloudQueueMessage>());

            var ip = new ImagePreprocessor(imaging, container, table, queue);
            await ip.Process(bytes, contentType, fileName);

            container.Received().Save(Arg.Any<string>(), bytes, contentType);
            table.Received().InsertOrReplace(Arg.Any<ImageEntity>());
            queue.Received().Save(Arg.Any<CloudQueueMessage>());
        }

        [Test]
        public async Task ProcessNoExtension()
        {
            var bytes = image;
            var contentType = Guid.NewGuid().ToString();
            var fileName = Guid.NewGuid().ToString();
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            container.Save(Arg.Any<string>(), bytes, contentType);
            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ImageEntity>());
            var queue = Substitute.For<IStorageQueue>();
            queue.Save(Arg.Any<CloudQueueMessage>());

            var ip = new ImagePreprocessor(imaging, container, table, queue);
            await ip.Process(bytes, contentType, fileName);

            container.Received().Save(Arg.Any<string>(), bytes, contentType);
            table.Received().InsertOrReplace(Arg.Any<ImageEntity>());
            queue.Received().Save(Arg.Any<CloudQueueMessage>());
        }
    }
}