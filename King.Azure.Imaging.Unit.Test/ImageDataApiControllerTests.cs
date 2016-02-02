namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Imaging.Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    [TestFixture]
    public class ImageDataApiControllerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new ImageDataApi(connectionString);
        }

        [Test]
        public void ConstructorElements()
        {
            new ImageDataApi(connectionString, new StorageElements());
        }

        [Test]
        public void ConstructorQueryDataStore()
        {
            var dataStore = Substitute.For<IQueryDataStore>();
            new ImageDataApi(dataStore);
        }

        [Test]
        public void ConstructorQueryDataStoreNull()
        {
            Assert.That(() => new ImageDataApi((IQueryDataStore)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsApiController()
        {
            Assert.IsNotNull(new ImageDataApi(connectionString) as ApiController);
        }

        [Test]
        public async Task Get()
        {
            var id =Guid.NewGuid();
            var version = Guid.NewGuid().ToString();
            var file = Guid.NewGuid().ToString();
            var items = new List<IDictionary<string, object>>();
            items.Add(new Dictionary<string, object>());

            var dataStore = Substitute.For<IQueryDataStore>();
            dataStore.Query(id, version, file).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(items));

            var c = new ImageDataApi(dataStore);
            var r = await c.Get(id, version, file);

            Assert.IsNotNull(r);
            Assert.AreEqual(r.StatusCode, HttpStatusCode.OK);

            dataStore.Received().Query(id, version, file);
        }

        [Test]
        public async Task GetReturnNull()
        {
            var id = Guid.NewGuid();
            var version = Guid.NewGuid().ToString();
            var file = Guid.NewGuid().ToString();
            var items = new List<IDictionary<string, object>>();
            items.Add(new Dictionary<string, object>());

            var dataStore = Substitute.For<IQueryDataStore>();
            dataStore.Query(id, version, file).Returns(Task.FromResult<IEnumerable<IDictionary<string, object>>>(null));

            var c = new ImageDataApi(dataStore);
            var r = await c.Get(id, version, file);

            Assert.IsNotNull(r);
            Assert.AreEqual(HttpStatusCode.NoContent, r.StatusCode);

            dataStore.Received().Query(id, version, file);
        }
    }
}