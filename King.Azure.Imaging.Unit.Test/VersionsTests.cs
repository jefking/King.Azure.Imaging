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
            Assert.IsNotNull(v.Images["Thumb"]);
            Assert.IsNotNull(v.Images["Medium"]);
            Assert.IsNotNull(v.Images["Large"]);
        }
    }
}