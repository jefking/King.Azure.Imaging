namespace King.Azure.Imaging
{
    using ImageProcessor;
    using ImageProcessor.Imaging.Formats;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// Imaging
    /// </summary>
    public class Imaging : IImaging
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Image Size</returns>
        public virtual Size Size(byte[] data)
        {
            var size = new Size();
            using (var image = new ImageFactory())
            using (var stream = new MemoryStream(data))
            {
                image.Load(stream);

                size.Height = image.Image.Height;
                size.Width = image.Image.Width;
            }

            return size;
        }

        /// <summary>
        /// Resize Image
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="version">Version</param>
        /// <param name="mimeType">Mime Type</param>
        /// <returns>Image Bytes</returns>
        public virtual byte[] Resize(byte[] data, IImageVersion version)
        {
            byte[] resized;
            using (var output = new MemoryStream())
            using (var input = new MemoryStream(data))
            using (var image = new ImageFactory(preserveExifData: true))
            {
                image.Load(input)
                    .Resize(new Size(version.Width, version.Height))
                    .Format(version.Format)
                    .Save(output);
                resized = output.ToArray();
            }

            return resized;
        }
        #endregion
    }
}