namespace King.Azure.Imaging.Unit.Test
{
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
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            new ImagingProcessor(container, table, new Dictionary<string, IImageVersion>());
        }

        [Test]
        public void IsIProcessor()
        {
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            Assert.IsNotNull(new ImagingProcessor(container, table, new Dictionary<string, IImageVersion>()) as IProcessor<ImageQueued>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            var table = Substitute.For<ITableStorage>();
            new ImagingProcessor(null, table, new Dictionary<string, IImageVersion>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var container = Substitute.For<IContainer>();
            new ImagingProcessor(container, null, new Dictionary<string, IImageVersion>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorVersionsNull()
        {
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            new ImagingProcessor(container, table, null);
        }

        [Test]
        public async Task Process()
        {
            var bytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\icon.png");
            var data = new ImageQueued()
            {
                Identifier = Guid.NewGuid(),
                FileNameFormat = "good_{0}_file",
            };
            var versions = new Dictionary<string, IImageVersion>();
            var version = new ImageVersion()
            {
                Width = 100,
                Height = 100,
            };
            versions.Add("temp", version);
            var container = Substitute.For<IContainer>();
            container.Get(string.Format(data.FileNameFormat, ImagePreprocessor.Original)).Returns(Task.FromResult(bytes));
            container.Save(string.Format(data.FileNameFormat, "temp"), Arg.Any<byte[]>(), "image/jpeg");

            var table = Substitute.For<ITableStorage>();
            table.InsertOrReplace(Arg.Any<ImageEntity>());

            var ip = new ImagingProcessor(container, table, versions);
            var result = await ip.Process(data);

            Assert.IsTrue(result);

            container.Received().Get(string.Format(data.FileNameFormat, ImagePreprocessor.Original));
            container.Received().Save(string.Format(data.FileNameFormat, "temp"), Arg.Any<byte[]>(), "image/jpeg");

            table.Received().InsertOrReplace(Arg.Any<ImageEntity>());
        }

        [Test]
        public async Task ProcessThrows()
        {
            var data = new ImageQueued()
            {
                Identifier = Guid.NewGuid(),
                FileNameFormat = "good_{0}_file",
            };
            var versions = new Dictionary<string, IImageVersion>();
            var version = new ImageVersion()
            {
                Width = 100,
                Height = 100,
            };
            versions.Add("temp", version);
            var container = Substitute.For<IContainer>();
            container.Get(string.Format(data.FileNameFormat, ImagePreprocessor.Original)).Returns(t => { throw new ArgumentException(); });

            var table = Substitute.For<ITableStorage>();

            var ip = new ImagingProcessor(container, table, versions);
            var result = await ip.Process(data);

            Assert.IsFalse(result);
        }
    }
}