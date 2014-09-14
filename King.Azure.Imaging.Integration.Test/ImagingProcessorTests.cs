namespace King.Azure.Imaging.Integration.Test
{
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImagingProcessorTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";
        private IContainer container;
        private ITableStorage table;

        [SetUp]
        public void Setup()
        {
            var name = 'a' + Guid.NewGuid().ToString().Replace('-', 'a').ToLowerInvariant();
            this.container = new Container(name, connectionString);
            this.container.CreateIfNotExists().Wait();
            this.table = new TableStorage(name, connectionString);
            this.table.CreateIfNotExists().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            this.container.Delete().Wait();
            this.table.Delete().Wait();
        }

        [Test]
        public async Task Process()
        {
            var random = new Random();
            var bytes = new byte[128];
            random.NextBytes(bytes);

            var resized = new byte[128];
            random.NextBytes(resized);

            var size = new Size()
            {
                Width = random.Next(),
                Height = random.Next(),
            };

            var versions = this.Versions();
            var version = versions.Values.First();

            var imaging = Substitute.For<IImaging>();
            imaging.Resize(Arg.Any<byte[]>(), version).Returns(resized);
            imaging.Size(resized).Returns(size);

            var queued = new ImageQueued
            {
                Identifier = Guid.NewGuid(),
                OriginalExtension = ImagePreprocessor.DefaultExtension,
                
            };
            queued.FileNameFormat = queued.Identifier.ToString() + "_{0}.{1}";

            await this.container.Save(string.Format("{0}_original.jpeg", queued.Identifier), bytes);

            var processor = new ImagingProcessor(imaging, this.container, this.table, versions);
            await processor.Process(queued);

            var data = await this.container.Get(string.Format("{0}_test.gif", queued.Identifier));
            Assert.AreEqual(resized, data);

            var entity = (from e in this.table.QueryByRow<ImageEntity>("test")
                          select e).FirstOrDefault();

            Assert.IsNotNull(entity);
            Assert.AreEqual(version.Format.MimeType, entity.ContentType);
            Assert.AreEqual(string.Format(ImagePreprocessor.PathFormat, this.container.Name, entity.FileName), entity.RelativePath);
            Assert.AreEqual(resized.LongLength, entity.FileSize);
            Assert.AreEqual(size.Width, entity.Width);
            Assert.AreEqual(size.Height, entity.Height);

            imaging.Received().Size(resized);
            imaging.Received().Resize(Arg.Any<byte[]>(), versions.Values.First());
        }

        private IDictionary<string, IImageVersion> Versions()
        {
            var versions = new Dictionary<string, IImageVersion>();
            var thumb = new ImageVersion
            {
                Width = 100,
                Format = new GifFormat { Quality = 10 },
            };
            versions.Add("test", thumb);
            return versions;
        }
    }
}