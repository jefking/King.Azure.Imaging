namespace King.Azure.Imaging.Integration.Test
{
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var f = dir.Substring(6, dir.Length - 6) + @"\icon.png";
            var bytes = File.ReadAllBytes(f);

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

            var processor = new Processor(new DataStore(connectionString), versions);
            await processor.Process(queued);

            var data = await this.container.Get(string.Format("{0}_test.gif", queued.Identifier));
            Assert.IsNotNull(data);

            var entities = await this.table.QueryByRow<ImageEntity>("test");
            var entity = entities.FirstOrDefault();

            Assert.IsNotNull(entity);
            Assert.AreEqual(version.Format.MimeType, entity.MimeType);
            Assert.AreEqual(string.Format(Naming.PathFormat, this.container.Name, entity.FileName), entity.RelativePath);
        }

        private IReadOnlyDictionary<string, IImageVersion> Versions()
        {
            var versions = new Dictionary<string, IImageVersion>();
            var thumb = new ImageVersion
            {
                Width = 100,
                Format = new GifFormat { Quality = 10 },
            };
            versions.Add("test", thumb);
            return new ReadOnlyDictionary<string, IImageVersion>(versions);
        }
    }
}