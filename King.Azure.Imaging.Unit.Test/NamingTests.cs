namespace King.Azure.Imaging.Unit.Test
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class NamingTests
    {
        [Test]
        public void Constructor()
        {
            new Naming();
        }

        [Test]
        public void IsINaming()
        {
            Assert.IsNotNull(new Naming() as INaming);
        }

        [Test]
        public void Original()
        {
            Assert.AreEqual("original", Naming.Original);
        }

        [Test]
        public void DynamicVersionFormat()
        {
            Assert.AreEqual("{0}_{1}_{2}x{3}", Naming.DynamicVersionFormat);
        }

        [Test]
        public void DefaultExtension()
        {
            Assert.AreEqual("jpeg", Naming.DefaultExtension);
        }

        [Test]
        public void FileNameFormat()
        {
            Assert.AreEqual("{0}_{1}.{2}", Naming.FileNameFormat);
        }

        [Test]
        public void PathFormat()
        {
            Assert.AreEqual("{0}/{1}", Naming.PathFormat);
        }

        [Test]
        public void DynamicVersion()
        {
            var random = new Random();
            var quality = random.Next();
            var width = random.Next();
            var height = random.Next();
            var extension = Guid.NewGuid().ToString();
            var n = new Naming();
            var result = n.DynamicVersion(extension, quality, width, height);
            var expected = string.Format(Naming.DynamicVersionFormat, extension, quality, width, height);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FileName()
        {
            var id = Guid.NewGuid();
            var version = Guid.NewGuid().ToString();
            var extension = Guid.NewGuid().ToString();
            var n = new Naming();
            var result = n.FileName(id, version, extension);
            Assert.AreEqual(string.Format(Naming.FileNameFormat, id, version, extension), result);
        }

        [Test]
        public void FileNamePartial()
        {
            var id = Guid.NewGuid();
            var n = new Naming();
            var result = n.FileNamePartial(id);
            Assert.IsTrue(result.StartsWith(id.ToString()));
            Assert.IsTrue(result.Contains("{0}"));
            Assert.IsTrue(result.Contains("{1}"));
        }

        [Test]
        public void FromFileName()
        {
            var fileName = Guid.NewGuid().ToString();
            var n = new Naming();
            var result = n.FromFileName(fileName + "_xxx.png");
            Assert.AreEqual(Guid.Parse(fileName), result);
        }

        [Test]
        public void Extension()
        {
            var fileName = Guid.NewGuid().ToString() + ".png";
            var n = new Naming();
            var result = n.Extension(fileName);
            Assert.AreEqual("png", result);
        }

        [Test]
        public void ExtensionDefault()
        {
            var fileName = Guid.NewGuid().ToString();
            var n = new Naming();
            var result = n.Extension(fileName);
            Assert.AreEqual(Naming.DefaultExtension, result);
        }

        [Test]
        public void RelativePath()
        {
            var folder = Guid.NewGuid().ToString();
            var file = Guid.NewGuid().ToString();
            var n = new Naming();
            var result = n.RelativePath(folder, file);
            Assert.AreEqual(folder + '/' + file, result);
        }
    }
}