namespace King.Azure.Imaging.Unit.Test.Tasks
{
    using King.Azure.Imaging.Models;
    using King.Azure.Imaging.Tasks;
    using King.Service;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class DequeueScalerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new DequeueScaler(new TaskConfiguration());
        }

        [Test]
        public void ConstructorConfigNull()
        {
            new DequeueScaler(null);
        }

        [Test]
        public void ScaleUnit()
        {
            var config = new TaskConfiguration
            {
                ConnectionString = connectionString,
                StorageElements = new StorageElements(),
                Versions = new Versions(),
            };
            
            var ds = new DequeueScaler(config);
            var task = ds.ScaleUnit(config);

            Assert.IsNotNull(task);
            Assert.AreEqual(1, task.Count());
            Assert.AreEqual(typeof(BackoffRunner), task.First().GetType());
        }
    }
}