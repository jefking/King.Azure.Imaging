namespace King.Azure.Imaging.Unit.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class NamingTests
    {
        [Test]
        public void Original()
        {
            Assert.AreEqual("original", Naming.Original);
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
    }
}