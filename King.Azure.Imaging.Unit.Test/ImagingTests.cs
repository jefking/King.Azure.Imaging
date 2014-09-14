namespace King.Azure.Imaging.Unit.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
    }
}