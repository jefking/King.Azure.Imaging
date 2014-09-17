namespace King.Azure.Imaging.Unit.Test.Models
{
    using King.Azure.Imaging.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ImageDataTests
    {
        [Test]
        public void Constructor()
        {
            new ImageData();
        }

        [Test]
        public void Raw()
        {
            var random = new Random();
            var expected = new byte[32];
            random.NextBytes(expected);

            var data = new ImageData()
            {
                Raw = expected
            };

            Assert.AreEqual(expected, data.Raw);
        }

        [Test]
        public void MimeType()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageData()
            {
                MimeType = expected
            };

            Assert.AreEqual(expected, data.MimeType);
        }
    }
}