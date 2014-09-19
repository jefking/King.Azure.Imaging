namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class DataStoreTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new DataStore(connectionString);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImagingNull()
        {
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            new DataStore(null, container, table, queue, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            var imaging = Substitute.For<IImaging>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            new DataStore(imaging, null, table, queue, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var queue = Substitute.For<IStorageQueue>();
            var naming = Substitute.For<INaming>();
            new DataStore(imaging, container, null, queue, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var naming = Substitute.For<INaming>();
            new DataStore(imaging, container, table, null, naming);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImageNamingNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            new DataStore(imaging, container, table, queue, null);
        }

        [Test]
        public void Streamer()
        {
            var store = new DataStore(connectionString);
            Assert.IsNotNull(store.Streamer);
        }
    }
}