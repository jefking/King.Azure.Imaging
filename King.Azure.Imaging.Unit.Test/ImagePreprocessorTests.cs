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
        public void DefaultExtension()
        {
            Assert.AreEqual("jpeg", ImagePreprocessor.DefaultExtension);
        }

        [Test]
        public void FileNameFormat()
        {
            Assert.AreEqual("{0}_{1}.{2}", ImagePreprocessor.FileNameFormat);
        }

        [Test]
        public void PathFormat()
        {
            Assert.AreEqual("{0}/{1}", ImagePreprocessor.PathFormat);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new ImagePreprocessor((string)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ProcessContentNull()
        {
            var connectionString = "UseDevelopmentStorage=true";

            var ip = new ImagePreprocessor(connectionString);
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

            var ip = new ImagePreprocessor(connectionString);
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

            var ip = new ImagePreprocessor(connectionString);
            await ip.Process(bytes, Guid.NewGuid().ToString(), null);
        }

        private static readonly byte[] image = File.ReadAllBytes(Environment.CurrentDirectory + "\\icon.png");

        [Test]
        public async Task Process()
        {
            var bytes = image;
            var contentType = Guid.NewGuid().ToString();
            var fileName = string.Format("{0}.png", Guid.NewGuid());
            var store = Substitute.For<IImageStore>();
            store.Save(Arg.Any<string>(), bytes, ImagePreprocessor.Original, contentType, Arg.Any<Guid>(), true, "png");

            var ip = new ImagePreprocessor(store);
            await ip.Process(bytes, contentType, fileName);

            store.Received().Save(Arg.Any<string>(), bytes, ImagePreprocessor.Original, contentType, Arg.Any<Guid>(), true, "png");
        }

        [Test]
        public async Task ProcessNoExtension()
        {
            var bytes = image;
            var contentType = Guid.NewGuid().ToString();
            var fileName = Guid.NewGuid().ToString();
            var store = Substitute.For<IImageStore>();
            store.Save(Arg.Any<string>(), bytes, ImagePreprocessor.Original, contentType, Arg.Any<Guid>(), true, "jpg");

            var ip = new ImagePreprocessor(store);
            await ip.Process(bytes, contentType, fileName);

            store.Received().Save(Arg.Any<string>(), bytes, ImagePreprocessor.Original, contentType, Arg.Any<Guid>(), true, "jpg");
        }
    }
}