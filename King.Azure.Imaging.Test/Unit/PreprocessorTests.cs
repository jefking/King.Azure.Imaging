namespace King.Azure.Imaging.Unit.Test
{
    using Azure.Imaging.Test.Integration;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    [TestFixture]
    public class PreprocessorTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new Preprocessor(connectionString);
        }

        [Test]
        public void IsIImagePreprocessor()
        {
            Assert.IsNotNull(new Preprocessor(connectionString) as IPreprocessor);
        }

        [Test]
        public void ConstructorConnectionStringNull()
        {
            Assert.That(() => new Preprocessor((string)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorDataStoreNull()
        {
            var naming = Substitute.For<INaming>();
            Assert.That(() => new Preprocessor(null, naming), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorCNamingNull()
        {
            var store = Substitute.For<IDataStore>();
            Assert.That(() => new Preprocessor(store, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ProcessContentNull()
        {
            var ip = new Preprocessor(connectionString);
            Assert.That(async () => await ip.Process(null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ProcessContentTypeNull()
        {
            var random = new Random();
            var bytes = new byte[64];
            random.NextBytes(bytes);
            
            var ip = new Preprocessor(connectionString);
            Assert.That(async () => await ip.Process(bytes, null, Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ProcessFileNameNull()
        {
            var random = new Random();
            var bytes = new byte[64];
            random.NextBytes(bytes);
            
            var ip = new Preprocessor(connectionString);
            Assert.That(async () => await ip.Process(bytes, Guid.NewGuid().ToString(), null), Throws.TypeOf<ArgumentException>());
        }
        
        [Test]
        public async Task Process()
        {
            var bytes = TestFile.Icon();

            var contentType = Guid.NewGuid().ToString();
            var fileName = string.Format("{0}.png", Guid.NewGuid());
            var store = Substitute.For<IDataStore>();
            store.Save("file.jpg", bytes, Naming.Original, contentType, Arg.Any<Guid>(), true, "png", 100);
            var naming = Substitute.For<INaming>();
            naming.Extension(fileName).Returns("png");
            naming.FileName(Arg.Any<Guid>(), Naming.Original, "png").Returns("file.png");

            var ip = new Preprocessor(store, naming);
            await ip.Process(bytes, contentType, fileName);

            naming.Received().Extension(fileName);
            naming.Received().FileName(Arg.Any<Guid>(), Naming.Original, "png");
            store.Received().Save("file.png", bytes, Naming.Original, contentType, Arg.Any<Guid>(), true, "png", 100);
        }

        [Test]
        public async Task ProcessNoExtension()
        {
            var bytes = TestFile.Icon();

            var contentType = Guid.NewGuid().ToString();
            var fileName = Guid.NewGuid().ToString();
            var store = Substitute.For<IDataStore>();
            store.Save("file.jpg", bytes, Naming.Original, contentType, Arg.Any<Guid>(), true, "jpg", 100);
            var naming = Substitute.For<INaming>();
            naming.Extension(fileName).Returns("jpg");
            naming.FileName(Arg.Any<Guid>(), Naming.Original, "jpg").Returns("file.jpg");

            var ip = new Preprocessor(store, naming);
            await ip.Process(bytes, contentType, fileName);

            naming.Received().Extension(fileName);
            naming.Received().FileName(Arg.Any<Guid>(), Naming.Original, "jpg");
            store.Received().Save("file.jpg", bytes, Naming.Original, contentType, Arg.Any<Guid>(), true, "jpg", 100);
        }
    }
}