namespace King.Azure.Imaging.Unit.Test
{
    using ImageProcessor.Imaging.Formats;
    using NUnit.Framework;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
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
        public void SizeDataNull()
        {
            var i = new Imaging();
            Assert.That(() => i.Size(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void SizeDataEmpty()
        {
            var i = new Imaging();
            Assert.That(() => i.Size(new byte[0]), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void DefaultImageQuality()
        {
            Assert.AreEqual(85, Imaging.DefaultImageQuality);
        }

        [Test]
        public void Size()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var file = dir.Substring(6, dir.Length - 6) + @"\icon.png";
            var bytes = File.ReadAllBytes(file);

            var i = new Imaging();
            var size = i.Size(bytes);

            Assert.IsNotNull(size);

            var bitMap = new Bitmap(file);
            Assert.AreEqual(bitMap.Width, size.Width);
            Assert.AreEqual(bitMap.Height, size.Height);
        }

        [Test]
        public void ResizeDataNull()
        {
            var i = new Imaging();
            Assert.That(() => i.Resize(null, new ImageVersion()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ResizeDataEmpty()
        {
            var i = new Imaging();
            Assert.That(() => i.Resize(new byte[0], new ImageVersion()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ResizeVersionNull()
        {
            var i = new Imaging();
            Assert.That(() => i.Resize(new byte[123], null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Resize()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var f = dir.Substring(6, dir.Length - 6) + @"\icon.png";
            var bytes = File.ReadAllBytes(f);
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