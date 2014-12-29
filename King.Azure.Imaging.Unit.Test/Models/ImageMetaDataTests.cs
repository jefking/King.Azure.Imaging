namespace King.Azure.Imaging.Unit.Test.Models
{
    using King.Azure.Imaging.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ImageMetaDataTests
    {
        [Test]
        public void Constructor()
        {
            new ImageMetaData();
        }

        [Test]
        public void ContentType()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageMetaData()
            {
                MimeType = expected
            };

            Assert.AreEqual(expected, data.MimeType);
        }

        [Test]
        public void RelativePath()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageMetaData()
            {
                RelativePath = expected
            };

            Assert.AreEqual(expected, data.RelativePath);
        }

        [Test]
        public void FileName()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageMetaData()
            {
                FileName = expected
            };

            Assert.AreEqual(expected, data.FileName);
        }

        [Test]
        public void FileSize()
        {
            var random = new Random();
            var expected = (uint)random.Next((int)uint.MinValue, (int)uint.MaxValue);

            var data = new ImageMetaData()
            {
                FileSize = expected
            };

            Assert.AreEqual(expected, data.FileSize);
        }

        [Test]
        public void Width()
        {
            var random = new Random();
            var expected = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);

            var data = new ImageMetaData()
            {
                Width = expected
            };

            Assert.AreEqual(expected, data.Width);
        }

        [Test]
        public void Height()
        {
            var random = new Random();
            var expected = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);

            var data = new ImageMetaData()
            {
                Height = expected
            };

            Assert.AreEqual(expected, data.Height);
        }

        [Test]
        public void Quality()
        {
            var random = new Random();
            var expected = (byte)random.Next(byte.MinValue, byte.MaxValue);

            var data = new ImageMetaData()
            {
                Quality = expected
            };

            Assert.AreEqual(expected, data.Quality);
        }
    }
}