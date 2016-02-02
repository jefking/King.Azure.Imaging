namespace King.Azure.Imaging.Unit.Test
{
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    [TestFixture]
    public class ProcessorTests
    {
        [Test]
        public void Constructor()
        {
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IDataStore>();
            new Processor(store, new Dictionary<string, IImageVersion>());
        }

        [Test]
        public void IsIProcessor()
        {
            var store = Substitute.For<IDataStore>();
            var imaging = Substitute.For<IImaging>();
            Assert.IsNotNull(new Processor(store, new Dictionary<string, IImageVersion>()) as IProcessor<ImageQueued>);
        }

        [Test]
        public void ConstructorImagingNull()
        {
            var store = Substitute.For<IDataStore>();
            var naming = Substitute.For<INaming>();
            Assert.That(() => new Processor(store, new Dictionary<string, IImageVersion>(), null, naming), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorNamingNull()
        {
            var store = Substitute.For<IDataStore>();
            var imaging = Substitute.For<IImaging>();
            Assert.That(() => new Processor(store, new Dictionary<string, IImageVersion>(), imaging, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorStoreeNull()
        {
            var imaging = Substitute.For<IImaging>();
            Assert.That(() => new Processor(null, new Dictionary<string, IImageVersion>()), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorVersionsNull()
        {
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IDataStore>();
            Assert.That(() => new Processor(store, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Process()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var f = dir.Substring(6, dir.Length - 6) + @"\icon.png";
            var bytes = File.ReadAllBytes(f);
            var data = new ImageQueued()
            {
                Identifier = Guid.NewGuid(),
                FileNameFormat = "good_{0}_file",
                OriginalExtension = string.Empty,
            };
            var versions = new Dictionary<string, IImageVersion>();
            var version = new ImageVersion()
            {
                Width = 100,
                Height = 100,
                Format = new JpegFormat { Quality = 70 },
            };
            versions.Add("temp", version);
            var fileName = string.Format(data.FileNameFormat, "temp");
            var original = string.Format(data.FileNameFormat, Naming.Original);

            var imaging = Substitute.For<IImaging>();
            imaging.Resize(bytes, version).Returns(bytes);
            var naming = Substitute.For<INaming>();
            naming.OriginalFileName(data).Returns(original);
            naming.FileName(data, "temp", version.Format.DefaultExtension).Returns(fileName);
            var streamer = Substitute.For<IStreamer>();
            streamer.Bytes(original).Returns(Task.FromResult(bytes));
            var store = Substitute.For<IDataStore>();
            store.Streamer.Returns(streamer);
            store.Save(fileName, bytes, "temp", version.Format.MimeType, data.Identifier, false, null, (byte)version.Format.Quality);

            var ip = new Processor(store, versions, imaging, naming);
            var result = await ip.Process(data);

            Assert.IsTrue(result);

            imaging.Received().Resize(bytes, version);
            naming.Received().OriginalFileName(data);
            naming.Received().FileName(data, "temp", version.Format.DefaultExtension);
            streamer.Received().Bytes(original);
            var s = store.Streamer.Received();
            store.Received().Save(fileName, bytes, "temp", version.Format.MimeType, data.Identifier, false, null, (byte)version.Format.Quality);
        }
    }
}