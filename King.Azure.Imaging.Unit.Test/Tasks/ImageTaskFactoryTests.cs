namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Imaging.Models;
    using King.Azure.Imaging.Tasks;
    using King.Service;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class ImageTaskFactoryTests
    {
        private const string connectionString = "UseDevelopmentStorage=true;";

        [Test]
        public void Constructor()
        {
            new ImageTaskFactory();
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new ImageTaskFactory() as ITaskFactory<ITaskConfiguration>);
        }

        [Test]
        public void TasksNull()
        {
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());
            var config = new TaskConfiguration
            {
                StorageElements = elements,
                Versions = Substitute.For<IVersions>(),
                ConnectionString = connectionString,
            };

            var factory = new ImageTaskFactory();
            var tasks = factory.Tasks(config);

            Assert.IsNotNull(tasks);
        }

        [Test]
        public void Tasks()
        {
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());
            var config = new TaskConfiguration
            {
                StorageElements = elements,
                Versions = Substitute.For<IVersions>(),
                ConnectionString = connectionString,
            };

            var factory = new ImageTaskFactory();
            var tasks = factory.Tasks(config);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(4, tasks.Count());
        }

        [Test]
        public void HasDequeueScaler()
        {
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());
            var config = new TaskConfiguration
            {
                StorageElements = elements,
                Versions = Substitute.For<IVersions>(),
                ConnectionString = connectionString,
            };

            var factory = new ImageTaskFactory();
            var tasks = factory.Tasks(config);

            Assert.IsNotNull(tasks);
            var task = (from t in tasks
                        where t.GetType() == typeof(DequeueScaler)
                        select t).FirstOrDefault();

            Assert.IsNotNull(task);
        }

        [Test]
        public void InitializeStorageTask()
        {
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());
            var config = new TaskConfiguration
            {
                StorageElements = elements,
                Versions = Substitute.For<IVersions>(),
                ConnectionString = connectionString,
            };

            var factory = new ImageTaskFactory();
            var tasks = factory.Tasks(config);

            Assert.IsNotNull(tasks);
            var inits = from t in tasks
                        where t.GetType() == typeof(InitializeStorageTask)
                        select t;

            Assert.IsNotNull(inits);
            Assert.AreEqual(3, inits.Count());
        }
    }
}