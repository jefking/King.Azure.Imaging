namespace King.Azure.Imaging.Unit.Test
{
    using King.Service;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorStorageElementsNull()
        {
            var versions = Substitute.For<IVersions>();
            new ImageTaskFactory(Guid.NewGuid().ToString(), null, versions);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorVersionsNull()
        {
            var elements = Substitute.For<IStorageElements>();
            new ImageTaskFactory(Guid.NewGuid().ToString(), elements, null);
        }
    }
}