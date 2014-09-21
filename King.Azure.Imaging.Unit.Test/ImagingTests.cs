namespace King.Azure.Imaging.Unit.Test
{
    using ImageProcessor.Imaging.Formats;
    using NUnit.Framework;
    using System;
    using System.Drawing;
    using System.IO;

    [TestFixture]
    public class ImagingTests
    {
        [Test]
        public void Constructor()
        {
            new Imaging();
        }

        [Test]
        public void IsIImaging()
        {
            Assert.IsNotNull(new Imaging() as IImaging);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SizeDataNull()
        {
            var i = new Imaging();
            i.Size(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SizeDataEmpty()
        {
            var i = new Imaging();
            i.Size(new byte[0]);
        }

        [Test]
        public void DefaultImageQuality()
        {
            Assert.AreEqual(85, Imaging.DefaultImageQuality);
        }

        [Test]
        public void Size()
        {
            var file = Environment.CurrentDirectory + @"\icon.png";
            var bytes = File.ReadAllBytes(file);

            var i = new Imaging();
            var size = i.Size(bytes);

            Assert.IsNotNull(size);

            var bitMap = new Bitmap(file);
            Assert.AreEqual(bitMap.Width, size.Width);
            Assert.AreEqual(bitMap.Height, size.Height);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ResizeDataNull()
        {
            var i = new Imaging();
            i.Resize(null, new ImageVersion());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ResizeDataEmpty()
        {
            var i = new Imaging();
            i.Resize(new byte[0], new ImageVersion());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResizeVersionNull()
        {
            var i = new Imaging();
            i.Resize(new byte[123], null);
        }

        [Test]
        public void Resize()
        {
            var bytes = File.ReadAllBytes(Environment.CurrentDirectory + @"\icon.png");
            var version = new ImageVersion()
            {
                Format = new GifFormat(),
                Width = 10,
                Height = 10,
            };

            var i = new Imaging();
            var data = i.Resize(bytes, version);

            Assert.IsNotNull(data);
            var size = i.Size(data);
            Assert.AreEqual(version.Width, size.Width);
            Assert.AreEqual(version.Height, size.Height);
        }

        [Test]
        public void GetDefault()
        {
            var i = new Imaging();
            var format = i.Get(null);
            Assert.IsNotNull(format as JpegFormat);
            format = i.Get(string.Empty);
            Assert.IsNotNull(format as JpegFormat);
            format = i.Get("  ");
            Assert.IsNotNull(format as JpegFormat);
        }

        [Test]
        public void GetInvalid()
        {
            var i = new Imaging();
            var format = i.Get(Guid.NewGuid().ToString());
            Assert.IsNotNull(format as JpegFormat);
        }

        [Test]
        public void GetBitMap()
        {
            var expected = new BitmapFormat();
            var i = new Imaging();
            foreach (var extension in expected.FileExtensions)
            {
                var format = i.Get(extension);
                Assert.AreEqual(expected.GetType(), format.GetType());
            }
        }

        [Test]
        public void GetTiff()
        {
            var expected = new TiffFormat();
            var i = new Imaging();
            foreach (var extension in expected.FileExtensions)
            {
                var format = i.Get(extension);
                Assert.AreEqual(expected.GetType(), format.GetType());
            }
        }

        [Test]
        public void GetPng()
        {
            var expected = new PngFormat();
            var i = new Imaging();
            foreach (var extension in expected.FileExtensions)
            {
                var format = i.Get(extension);
                Assert.AreEqual(expected.GetType(), format.GetType());
            }
        }

        [Test]
        public void GetJpeg()
        {
            var expected = new JpegFormat();
            var i = new Imaging();
            foreach (var extension in expected.FileExtensions)
            {
                var format = i.Get(extension);
                Assert.AreEqual(expected.GetType(), format.GetType());
            }
        }

        [Test]
        public void GetGif()
        {
            var expected = new GifFormat();
            var i = new Imaging();
            foreach (var extension in expected.FileExtensions)
            {
                var format = i.Get(extension);
                Assert.AreEqual(expected.GetType(), format.GetType());
            }
        }

        [Test]
        public void GetNegativeQuality()
        {
            var expected = new GifFormat();
            var i = new Imaging();
            foreach (var extension in expected.FileExtensions)
            {
                var format = i.Get(extension, -45);
                Assert.AreEqual(Imaging.DefaultImageQuality, format.Quality);
            }
        }
    }
}