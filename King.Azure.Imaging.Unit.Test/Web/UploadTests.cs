namespace King.Azure.Imaging.Unit.Test.Web
{
    using King.Azure.Imaging.Web;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class UploadTests
    {
        [Test]
        public void Constructor()
        {
            new Upload();
        }

        [Test]
        public void FileNameHeader()
        {
            Assert.AreEqual("X-File-Name", Upload.FileNameHeader);
        }

        [Test]
        public void ContentTypeHeader()
        {
            Assert.AreEqual("X-File-Type", Upload.ContentTypeHeader);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task LoadRequestNull()
        {
            var u = new Upload();
            await u.Load(null);
        }
    }
}