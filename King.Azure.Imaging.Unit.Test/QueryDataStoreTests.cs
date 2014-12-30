namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class QueryDataStoreTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new QueryDataStore(connectionString);
        }

        [Test]
        public void ConstructorElements()
        {
            new QueryDataStore(connectionString, new StorageElements());
        }

        [Test]
        public void ConstructorQueryDataStore()
        {
            var table = Substitute.For<ITableStorage>();
            new QueryDataStore(table);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueryDataStoreNull()
        {
            new QueryDataStore((ITableStorage)null);
        }

        [Test]
        public void IsIQueryDataStore()
        {
            Assert.IsNotNull(new QueryDataStore(connectionString) as IQueryDataStore);
        }

        [Test]
        public async Task Query()
        {
            var id = Guid.NewGuid();
            var version = Guid.NewGuid().ToString();
            var fileName = Guid.NewGuid().ToString();
            var etag = Guid.NewGuid().ToString();
            var createdOn = DateTime.UtcNow;

            var items = new List<IDictionary<string, object>>();
            var dic = new Dictionary<string, object>();
            dic.Add(TableStorage.PartitionKey, id);
            dic.Add(TableStorage.RowKey, version);
            dic.Add(TableStorage.Timestamp, createdOn);
            dic.Add(TableStorage.ETag, etag);
            dic.Add("FileName", fileName);

            items.Add(dic);

            var table = Substitute.For<ITableStorage>();
            table.Query(Arg.Any<TableQuery>()).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(items));

            var store = new QueryDataStore(table);
            var r = await store.Query(id, version, fileName);

            Assert.IsNotNull(r);
            Assert.AreEqual(1, r.Count());
            var d = r.First();
            Assert.IsFalse(d.ContainsKey(TableStorage.PartitionKey));
            Assert.IsFalse(d.ContainsKey(TableStorage.RowKey));
            Assert.IsFalse(d.ContainsKey(TableStorage.Timestamp));
            Assert.IsFalse(d.ContainsKey(TableStorage.ETag));
            Assert.AreEqual(id, d["Identifier"]);
            Assert.AreEqual(version, d["Version"]);
            Assert.AreEqual(createdOn, d["CreatedOn"]);

            table.Received().Query(Arg.Any<TableQuery>());
        }

        [Test]
        public async Task QueryFilterOnFileName()
        {
            var id = Guid.NewGuid();
            var version = Guid.NewGuid().ToString();
            var fileName = Guid.NewGuid().ToString();
            var etag = Guid.NewGuid().ToString();
            var createdOn = DateTime.UtcNow;

            var items = new List<IDictionary<string, object>>();
            var dic = new Dictionary<string, object>();
            dic.Add(TableStorage.PartitionKey, id);
            dic.Add(TableStorage.RowKey, version);
            dic.Add(TableStorage.Timestamp, createdOn);
            dic.Add(TableStorage.ETag, etag);
            dic.Add("FileName", fileName);
            items.Add(dic);

            var random = new Random();
            var count = random.Next(1, 25);
            for (var i = 0; i < count; i++)
            {
                var filtered = new Dictionary<string, object>();
                filtered.Add(TableStorage.PartitionKey, id);
                filtered.Add(TableStorage.RowKey, version);
                filtered.Add(TableStorage.Timestamp, createdOn);
                filtered.Add(TableStorage.ETag, etag);
                filtered.Add("FileName", Guid.NewGuid().ToString());
                items.Add(filtered);
            }

            var table = Substitute.For<ITableStorage>();
            table.Query(Arg.Any<TableQuery>()).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(items));

            var store = new QueryDataStore(table);
            var r = await store.Query(id, version, fileName);

            Assert.IsNotNull(r);
            Assert.AreEqual(1, r.Count());
            var d = r.First();
            Assert.IsFalse(d.ContainsKey(TableStorage.PartitionKey));
            Assert.IsFalse(d.ContainsKey(TableStorage.RowKey));
            Assert.IsFalse(d.ContainsKey(TableStorage.Timestamp));
            Assert.IsFalse(d.ContainsKey(TableStorage.ETag));
            Assert.AreEqual(id, d["Identifier"]);
            Assert.AreEqual(version, d["Version"]);
            Assert.AreEqual(createdOn, d["CreatedOn"]);

            table.Received().Query(Arg.Any<TableQuery>());
        }

        [Test]
        public async Task QueryFileNameNoFiltered()
        {
            var id = Guid.NewGuid();
            var version = Guid.NewGuid().ToString();
            var etag = Guid.NewGuid().ToString();
            var createdOn = DateTime.UtcNow;

            var random = new Random();
            var count = random.Next(1, 25);
            var items = new List<IDictionary<string, object>>(count);
            for (var i = 0; i < count; i++)
            {
                var filtered = new Dictionary<string, object>();
                filtered.Add(TableStorage.PartitionKey, id);
                filtered.Add(TableStorage.RowKey, version);
                filtered.Add(TableStorage.Timestamp, createdOn);
                filtered.Add(TableStorage.ETag, etag);
                filtered.Add("FileName", Guid.NewGuid().ToString());
                items.Add(filtered);
            }

            var table = Substitute.For<ITableStorage>();
            table.Query(Arg.Any<TableQuery>()).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(items));

            var store = new QueryDataStore(table);
            var r = await store.Query(id, version, null);

            Assert.IsNotNull(r);
            Assert.AreEqual(count, r.Count());

            table.Received().Query(Arg.Any<TableQuery>());
        }

        [Test]
        public async Task QueryReturnsNull()
        {
            var id = Guid.NewGuid();
            var version = Guid.NewGuid().ToString();
            var fileName = Guid.NewGuid().ToString();

            var table = Substitute.For<ITableStorage>();
            table.Query(Arg.Any<TableQuery>()).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(null));

            var store = new QueryDataStore(table);
            var r = await store.Query(id, version, fileName);

            Assert.IsNull(r);

            table.Received().Query(Arg.Any<TableQuery>());
        }

        [Test]
        public async Task QueryNoPartitionReturnsNull()
        {
            var version = Guid.NewGuid().ToString();
            var fileName = Guid.NewGuid().ToString();

            var table = Substitute.For<ITableStorage>();
            table.Query(Arg.Any<TableQuery>()).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(null));

            var store = new QueryDataStore(table);
            var r = await store.Query(null, version, fileName);

            Assert.IsNull(r);

            table.Received().Query(Arg.Any<TableQuery>());
        }

        [Test]
        public async Task QueryNoRowReturnsNull()
        {
            var id = Guid.NewGuid();
            var fileName = Guid.NewGuid().ToString();

            var table = Substitute.For<ITableStorage>();
            table.Query(Arg.Any<TableQuery>()).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(null));

            var store = new QueryDataStore(table);
            var r = await store.Query(id, null, fileName);

            Assert.IsNull(r);

            table.Received().Query(Arg.Any<TableQuery>());
        }
    }
}