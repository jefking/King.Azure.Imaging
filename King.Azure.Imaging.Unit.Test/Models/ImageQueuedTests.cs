namespace King.Azure.Imaging.Unit.Test.Models
{
    using King.Azure.Imaging.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ImageQueuedTests
    {
        [Test]
        public void Constructor()
        {
            new ImageQueued();
        }

        [Test]
        public void Identifier()
        {
            var expected = Guid.NewGuid();
            var data = new ImageQueued()
            {
                Identifier = expected
            };

            Assert.AreEqual(expected, data.Identifier);
        }

        [Test]
        public void OriginalExtension()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageQueued()
            {
                OriginalExtension = expected
            };

            Assert.AreEqual(expected, data.OriginalExtension);
        }

        [Test]
        public void FileNameFormat()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageQueued()
            {
                FileNameFormat = expected
            };

            Assert.AreEqual(expected, data.FileNameFormat);
        }
    }
}