﻿namespace King.Azure.Imaging.Unit.Test.Entities
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
        public void RelativePath()
        {
            var expected = Guid.NewGuid().ToString();
            var data = new ImageEntity()
            {
                RelativePath = expected
            };

            Assert.AreEqual(expected, data.RelativePath);
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

        [Test]
        public void Width()
        {
            var random = new Random();
            var expected = random.Next();

            var data = new ImageEntity()
            {
                Width = expected
            };

            Assert.AreEqual(expected, data.Width);
        }

        [Test]
        public void Height()
        {
            var random = new Random();
            var expected = random.Next();

            var data = new ImageEntity()
            {
                Height = expected
            };

            Assert.AreEqual(expected, data.Height);
        }

        [Test]
        public void Quality()
        {
            var random = new Random();
            var expected = random.Next();

            var data = new ImageEntity()
            {
                Quality = expected
            };

            Assert.AreEqual(expected, data.Quality);
        }
    }
}