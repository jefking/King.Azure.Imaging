namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using System.IO;
    using System.Threading.Tasks;

    public class ImageStreamer : IImageStreamer
    {
        private readonly IContainer container;

        public ImageStreamer(IContainer container)
        {
            this.container = container;
        }

        public async Task<Stream> Get(string file)
        {
            var bytes = await container.Get(file);
            var ms = new MemoryStream();
            await ms.WriteAsync(bytes, 0, bytes.Length);
            ms.Position = 0;

            var properties = await container.Properties(file);
            this.ContentType = properties.ContentType;

            return ms;
        }

        public string ContentType
        {
            private set;
            get;
        }
    }
}
