namespace King.Azure.Imaging
{
    using ImageProcessor;
    using ImageProcessor.Configuration;
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
        /// <remarks>
        /// Threading issues when made static
        /// </remarks>
        protected readonly IEnumerable<ISupportedImageFormat> formats = ImageProcessorBootstrapper.Instance.SupportedImageFormats;

        /// <summary>
        /// Default Image Quality
        /// </summary>
        public const byte DefaultImageQuality = 85;
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

            byte[] resized = null;
            using (var output = new MemoryStream())
            using (var input = new MemoryStream(data))
            using (var image = new ImageFactory(true))
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
        /// <param name="quality">Quality Settings</param>
        /// <returns>Image Format</returns>
        public virtual ISupportedImageFormat Get(string extension = Naming.DefaultExtension, int quality = DefaultImageQuality)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return new JpegFormat()
                {
                    Quality = quality
                };
            }

            quality = quality > 0 ? quality : DefaultImageQuality;
            extension = extension.ToLowerInvariant();

            foreach (var format in formats)
            {
                var isFormat = (from e in format.FileExtensions
                                where extension == e.ToLowerInvariant()
                                select true).FirstOrDefault();

                if (isFormat)
                {
                    var temp = Activator.CreateInstance(format.GetType()) as ISupportedImageFormat;
                    temp.Quality = quality;
                    return temp;
                }
            }

            return new JpegFormat()
            {
                Quality = quality
            };
        }
        #endregion
    }
}