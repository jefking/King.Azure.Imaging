namespace King.Azure.Imaging.Unit.Test
{
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class VersionsTests
    {
        [Test]
        public void Constructor()
        {
            new Versions();
        }

        [Test]
        public void IsIVersions()
        {
            Assert.IsNotNull(new Versions() as IVersions);
        }

        [Test]
        public void Images()
        {
            var v = new Versions();
            Assert.IsNotNull(v.Images);
            Assert.AreEqual(3, v.Images.Keys.Count());
            Assert.IsNotNull(v.Images["thumb"]);
            Assert.IsNotNull(v.Images["medium"]);
            Assert.IsNotNull(v.Images["large"]);
        }

        [Test]
        public void Thumb()
        {
            var v = new Versions();
            var version = v.Images["thumb"];
            Assert.AreEqual(100, version.Width);
            Assert.AreEqual(0, version.Height);
            Assert.AreEqual(50, version.Format.Quality);
        }

        [Test]
        public void Medium()
        {
            var v = new Versions();
            var version = v.Images["medium"];
            Assert.AreEqual(640, version.Width);
            Assert.AreEqual(0, version.Height);
            Assert.AreEqual(70, version.Format.Quality);
        }

        [Test]
        public void Large()
        {
            var v = new Versions();
            var version = v.Images["large"];
            Assert.AreEqual(1080, version.Width);
            Assert.AreEqual(0, version.Height);
            Assert.AreEqual(85, version.Format.Quality);
        }
    }
}