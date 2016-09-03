namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Imaging.Models;
    using NUnit.Framework;

    [TestFixture]
    public class StorageElementsTests
    {
        [Test]
        public void Constructor()
        {
            new StorageElements();
        }

        [Test]
        public void IStorageElements()
        {
            Assert.IsNotNull(new StorageElements() as IStorageElements);
        }

        [Test]
        public void Container()
        {
            var data = new StorageElements();
            Assert.AreEqual("images", data.Container);
        }

        [Test]
        public void Queue()
        {
            var data = new StorageElements();
            Assert.AreEqual("imaging", data.Queue);
        }

        [Test]
        public void Table()
        {
            var data = new StorageElements();
            Assert.AreEqual("imaging", data.Table);
        }
    }
}