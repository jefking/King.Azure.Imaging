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
        public void ConstructorContainerNull()
        {
            Assert.That(() => new Streamer(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetFileNull()
        {
            var container = Substitute.For<IContainer>();
            var istreamer = new Streamer(container);
            Assert.That(async () => await istreamer.Stream(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetBytesNull()
        {
            var container = Substitute.For<IContainer>();
            var istreamer = new Streamer(container);
            Assert.That(async () => await istreamer.Bytes(null), Throws.TypeOf<ArgumentException>());
        }
    }
}