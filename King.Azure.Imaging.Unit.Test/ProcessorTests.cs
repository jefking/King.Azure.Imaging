namespace King.Azure.Imaging.Unit.Test
{
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    [TestFixture]
    public class ProcessorTests
    {
        [Test]
        public void Constructor()
        {
            var imaging = Substitute.For<IImaging>();
            var table = Substitute.For<ITableStorage>();
            var store = Substitute.For<IDataStore>();
            new Processor(imaging, store, new Dictionary<string, IImageVersion>());
        }

        [Test]
        public void IsIProcessor()
        {
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IDataStore>();
            Assert.IsNotNull(new Processor(imaging, store, new Dictionary<string, IImageVersion>()) as IProcessor<ImageQueued>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImagingNull()
        {
            var store = Substitute.For<IDataStore>();
            new Processor(null, store, new Dictionary<string, IImageVersion>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorStoreeNull()
        {
            var imaging = Substitute.For<IImaging>();
            new Processor(imaging, null, new Dictionary<string, IImageVersion>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorVersionsNull()
        {
            var imaging = Substitute.For<IImaging>();
            var store = Substitute.For<IDataStore>();
            new Processor(imaging, store, null);
        }

        [Test]
        public async Task Process()
        {
            var bytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\icon.png");
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
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            container.Get(string.Format(data.FileNameFormat, Naming.Original)).Returns(Task.FromResult(bytes));
            container.Save(string.Format(data.FileNameFormat, "temp"), Arg.Any<byte[]>(), version.Format.MimeType);

            var store = Substitute.For<IDataStore>();

            var ip = new Processor(imaging, store, versions);
            var result = await ip.Process(data);

            Assert.IsTrue(result);

            container.Received().Get(string.Format(data.FileNameFormat, Naming.Original));
            container.Received().Save(string.Format(data.FileNameFormat, "temp"), Arg.Any<byte[]>(), version.Format.MimeType);
        }
    }
}