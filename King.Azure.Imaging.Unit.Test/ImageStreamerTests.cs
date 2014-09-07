namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ImageStreamerTests
    {
        [Test]
        public void Constructor()
        {
            var container = Substitute.For<IContainer>();
            new ImageStreamer(container);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            new ImageStreamer(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFileNull()
        {
            var container = Substitute.For<IContainer>();
            var istreamer =new ImageStreamer(container);
            await istreamer.Get(null);
        }
    }
}