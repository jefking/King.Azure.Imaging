namespace King.Azure.Imaging.Unit.Test.Entities
{
    using King.Azure.Imaging.Entities;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ImageEntityTests
    {
        [Test]
        public void Constructor()
        {
            new ImageEntity();
        }

        [Test]
        public void ContentType()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageEntity()
            {
                ContentType = expected
            };

            Assert.AreEqual(expected, data.ContentType);
        }

        [Test]
        public void FileName()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageEntity()
            {
                FileName = expected
            };

            Assert.AreEqual(expected, data.FileName);
        }

        [Test]
        public void FileSize()
        {
            var random = new Random();
            var expected = random.Next();

            var data = new ImageEntity()
            {
                FileSize = expected
            };

            Assert.AreEqual(expected, data.FileSize);
        }
    }
}