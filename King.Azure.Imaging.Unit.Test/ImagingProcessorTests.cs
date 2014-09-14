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
    public class ImagingProcessorTests
    {
        [Test]
        public void Constructor()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            new ImagingProcessor(imaging, container, table, new Dictionary<string, IImageVersion>());
        }

        [Test]
        public void IsIProcessor()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new ImagingProcessor(imaging, container, table, new Dictionary<string, IImageVersion>()) as IProcessor<ImageQueued>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImagingNull()
        {
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            new ImagingProcessor(null, container, table, new Dictionary<string, IImageVersion>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            var imaging = Substitute.For<IImaging>();
            var table = Substitute.For<ITableStorage>();
            new ImagingProcessor(imaging, null, table, new Dictionary<string, IImageVersion>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            new ImagingProcessor(imaging, container, null, new Dictionary<string, IImageVersion>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorVersionsNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            new ImagingProcessor(imaging, container, table, null);
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
            container.Get(string.Format(data.FileNameFormat, ImagePreprocessor.Original)).Returns(Task.FromResult(bytes));
            container.Save(string.Format(data.FileNameFormat, "temp"), Arg.Any<byte[]>(), version.Format.MimeType);

            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ImageEntity>());

            var ip = new ImagingProcessor(imaging, container, table, versions);
            var result = await ip.Process(data);

            Assert.IsTrue(result);

            container.Received().Get(string.Format(data.FileNameFormat, ImagePreprocessor.Original));
            container.Received().Save(string.Format(data.FileNameFormat, "temp"), Arg.Any<byte[]>(), version.Format.MimeType);

            table.Received().InsertOrReplace(Arg.Any<ImageEntity>());
        }

        [Test]
        public async Task ProcessThrows()
        {
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
            };
            versions.Add("temp", version);
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            container.Get(string.Format(data.FileNameFormat, ImagePreprocessor.Original)).Returns(t => { throw new ArgumentException(); });

            var table = Substitute.For<ITableStorage>();

            var ip = new ImagingProcessor(imaging, container, table, versions);
            var result = await ip.Process(data);

            Assert.IsFalse(result);
        }
    }
}