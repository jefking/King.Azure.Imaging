namespace King.Azure.Imaging.Unit.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImagePreprocessorTests
    {
        [Test]
        public void Constructor()
        {
            var connectionString = "UseDevelopmentStorage=true";
            new ImagePreprocessor(connectionString);
        }

        [Test]
        public void IsIImagePreprocessor()
        {
            var connectionString = "UseDevelopmentStorage=true";
            Assert.IsNotNull(new ImagePreprocessor(connectionString) as IImagePreprocessor);
        }

        [Test]
        public void Original()
        {
            Assert.AreEqual("original", ImagePreprocessor.Original);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new ImagePreprocessor(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorElementsNull()
        {
            var connectionString = "UseDevelopmentStorage=true";
            new ImagePreprocessor(connectionString, null);
        }
    }
}