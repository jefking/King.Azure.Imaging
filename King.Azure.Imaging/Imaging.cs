namespace King.Azure.Imaging
{
    using ImageProcessor;
    using ImageProcessor.Imaging.Formats;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Imaging, wrapper for ImageProcessor tasks
    /// </summary>
    public class Imaging : IImaging
    {
        #region Members
        /// <summary>
        /// Image Formats
        /// </summary>
        protected static readonly IEnumerable<ISupportedImageFormat> formats = new ISupportedImageFormat[] { new BitmapFormat(), new GifFormat(), new JpegFormat(), new PngFormat(), new TiffFormat() };
        #endregion

        #region Methods
        /// <summary>
        /// Dimensions of Image
        /// </summary>
        /// <param name="data">Image Bytes</param>
        /// <returns>Image Size</returns>
        public virtual Size Size(byte[] data)
        {
            if (null == data || !data.Any())
            {
                throw new ArgumentException("data");
            }

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
        /// <returns>Image Bytes</returns>
        public virtual byte[] Resize(byte[] data, IImageVersion version)
        {
            if (null == data || !data.Any())
            {
                throw new ArgumentException("data");
            }
            if (null == version)
            {
                throw new ArgumentNullException("version");
            }

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

        /// <summary>
        /// Get Image Format
        /// </summary>
        /// <param name="extension">Extension</param>
        /// <returns>Image Format</returns>
        public virtual ISupportedImageFormat Get(string extension = ImagePreprocessor.DefaultExtension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return new JpegFormat();
            }

            foreach (var format in formats)
            {
                var f = (from e in format.FileExtensions
                         where extension == e
                         select e).FirstOrDefault();

                if (null != f)
                {
                    return format;
                }
            }

            return new JpegFormat();
        }
        #endregion
    }
}