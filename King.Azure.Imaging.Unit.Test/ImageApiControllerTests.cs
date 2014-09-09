namespace King.Azure.Imaging.Unit.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    [TestFixture]
    public class ImageApiControllerTests
    {
        private const string connectionString = "UseDevelopmentStorage=true";

        [Test]
        public void Constructor()
        {
            new ImageApiController(connectionString);
        }

        [Test]
        public void IsApiController()
        {
            Assert.IsNotNull(new ImageApiController(connectionString) as ApiController);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            new ImageApiController(null, preprocessor, elements);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorImagePreprocessorNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            var elements = Substitute.For<IStorageElements>();
            new ImageApiController(connectionString, null, elements);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorStorageElementsNull()
        {
            var preprocessor = Substitute.For<IImagePreprocessor>();
            new ImageApiController(connectionString, preprocessor, null);
        }
    }
}