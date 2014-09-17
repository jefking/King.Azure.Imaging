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

    public class ImageStoreTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorImagingNull()
        {
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            new ImageStore(null, container, table, queue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            var imaging = Substitute.For<IImaging>();
            var table = Substitute.For<ITableStorage>();
            var queue = Substitute.For<IStorageQueue>();
            new ImageStore(imaging, null, table, queue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTableNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var queue = Substitute.For<IStorageQueue>();
            new ImageStore(imaging, container, null, queue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            var imaging = Substitute.For<IImaging>();
            var container = Substitute.For<IContainer>();
            var table = Substitute.For<ITableStorage>();
            new ImageStore(imaging, container, table, null);
        }
    }
}