namespace King.Azure.Imaging.Unit.Test.Tasks
{
    using King.Azure.Imaging.Models;
    using King.Azure.Imaging.Tasks;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ImageDequeueSetupTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            var config = new TaskConfiguration()
            {
                Versions = new Versions(),
                StorageElements = new StorageElements(),
            };

            new ImageDequeueSetup(config);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorConfigNull()
        {
            new ImageDequeueSetup(null);
        }

        [Test]
        public void Get()
        {
            var config = new TaskConfiguration()
            {
                Versions = new Versions(),
                StorageElements = new StorageElements(),
                ConnectionString = connectionString,
            };

            var setup = new ImageDequeueSetup(config);
            var p = setup.Processor();

            Assert.IsNotNull(p as Processor);
        }
    }
}