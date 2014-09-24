namespace King.Azure.Imaging.Unit.Test
{
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class StreamerTests
    {
        [Test]
        public void Constructor()
        {
            var container = Substitute.For<IContainer>();
            new Streamer(container);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            new Streamer(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFileNull()
        {
            var container = Substitute.For<IContainer>();
            var istreamer = new Streamer(container);
            await istreamer.Stream(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetBytesNull()
        {
            var container = Substitute.For<IContainer>();
            var istreamer = new Streamer(container);
            await istreamer.Bytes(null);
        }
    }
}