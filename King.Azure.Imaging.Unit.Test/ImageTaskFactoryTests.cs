namespace King.Azure.Imaging.Unit.Test
{
    using King.Service;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;

    [TestFixture]
    public class ImageTaskFactoryTests
    {
        [Test]
        public void Constructor()
        {
            new ImageTaskFactory(Guid.NewGuid().ToString());
        }

        [Test]
        public void IsITaskFactory()
        {
            Assert.IsNotNull(new ImageTaskFactory(Guid.NewGuid().ToString()) as ITaskFactory<object>);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new ImageTaskFactory(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorStorageElementsNull()
        {
            var versions = Substitute.For<IVersions>();
            new ImageTaskFactory(Guid.NewGuid().ToString(), versions, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorVersionsNull()
        {
            var elements = Substitute.For<IStorageElements>();
            new ImageTaskFactory(Guid.NewGuid().ToString(), null, elements);
        }

        [Test]
        public void TasksNull()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());

            var versions = Substitute.For<IVersions>();

            var factory = new ImageTaskFactory(connectionString, versions, elements);
            var tasks = factory.Tasks(null);

            Assert.IsNotNull(tasks);
        }

        [Test]
        public void Tasks()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());
            var versions = Substitute.For<IVersions>();

            var factory = new ImageTaskFactory(connectionString, versions, elements);
            var tasks = factory.Tasks(null);

            Assert.IsNotNull(tasks);
            Assert.AreEqual(4, tasks.Count());
        }

        [Test]
        public void HasBackoffRunner()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());
            var versions = Substitute.For<IVersions>();

            var factory = new ImageTaskFactory(connectionString, versions, elements);
            var tasks = factory.Tasks(null);

            Assert.IsNotNull(tasks);
            var task = (from t in tasks
                        where t.GetType() == typeof(BackoffRunner)
                        select t).FirstOrDefault();

            Assert.IsNotNull(task);
        }

        [Test]
        public void InitializeStorageTask()
        {
            var connectionString = "UseDevelopmentStorage=true";
            var elements = Substitute.For<IStorageElements>();
            elements.Container.Returns(Guid.NewGuid().ToString());
            elements.Table.Returns(Guid.NewGuid().ToString());
            elements.Queue.Returns(Guid.NewGuid().ToString());
            var versions = Substitute.For<IVersions>();

            var factory = new ImageTaskFactory(connectionString, versions, elements);
            var tasks = factory.Tasks(null);

            Assert.IsNotNull(tasks);
            var inits = from t in tasks
                        where t.GetType() == typeof(InitializeStorageTask)
                        select t;

            Assert.IsNotNull(inits);
            Assert.AreEqual(3, inits.Count());
        }
    }
}