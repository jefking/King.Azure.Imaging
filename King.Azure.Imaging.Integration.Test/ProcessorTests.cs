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
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class ProcessorTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";
        private IContainer container;
        private ITableStorage table;

        [SetUp]
        public void Setup()
        {
            var elements = new StorageElements();
            this.container = new Container(elements.Container, connectionString);
            this.container.CreateIfNotExists().Wait();
            this.table = new TableStorage(elements.Table, connectionString);
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
            var bytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\icon.png");

            var versions = this.Versions();
            var version = versions.Values.First();

            var queued = new ImageQueued
            {
                Identifier = Guid.NewGuid(),
                OriginalExtension = Naming.DefaultExtension,
                
            };
            queued.FileNameFormat = queued.Identifier.ToString() + "_{0}.{1}";

            await this.container.Save(string.Format("{0}_original.jpeg", queued.Identifier), bytes);

            var store = new DataStore(connectionString);

            var processor = new Processor(new Imaging(), new DataStore(connectionString), versions);
            await processor.Process(queued);

            var data = await this.container.Get(string.Format("{0}_test.gif", queued.Identifier));
            Assert.IsNotNull(data);

            var entity = (from e in this.table.QueryByRow<ImageEntity>("test")
                          select e).FirstOrDefault();

            Assert.IsNotNull(entity);
            Assert.AreEqual(version.Format.MimeType, entity.ContentType);
            Assert.AreEqual(string.Format(Naming.PathFormat, this.container.Name, entity.FileName), entity.RelativePath);
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