namespace King.Azure.Imaging.Unit.Test.Models
{
    using King.Azure.Imaging.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class RawDataTests
    {
        [Test]
        public void Contents()
        {
            var random = new Random();
            var expected = new byte[128];
            random.NextBytes(expected);

            var data = new RawData()
            {
                Contents = expected
            };

            Assert.AreEqual(expected, data.Contents);
        }

        [Test]
        public void ContentType()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new RawData()
            {
                ContentType = expected
            };

            Assert.AreEqual(expected, data.ContentType);
        }

        [Test]
        public void FileName()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new RawData()
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

            var data = new RawData()
            {
                FileSize = expected
            };

            Assert.AreEqual(expected, data.FileSize);
        }

        [Test]
        public void Identifier()
        {
            var expected = Guid.NewGuid();
            var data = new RawData()
            {
                Identifier = expected
            };

            Assert.AreEqual(expected, data.Identifier);
        }
    }
}