﻿namespace King.Azure.Imaging
{
    using ImageProcessor;
    using ImageProcessor.Imaging.Formats;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// Imaging
    /// </summary>
    public class Imaging
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual Size Size(byte[] data)
        {
            var size = new Size();
            using (var image = new ImageFactory())
            {
                using (var stream = new MemoryStream(data))
                {
                    image.Load(stream);
                    size.Height = image.Image.Height;
                    size.Width = image.Image.Width;
                }
            }
            return size;
        }

        public virtual byte[] Resize(byte[] data, IImageVersion version, out string mimeType)
        {
            byte[] resized;
            var format = new JpegFormat { Quality = 70 };//Make Dynamic
            mimeType = format.MimeType;
            var size = new Size(version.Width, version.Height);
            using (var input = new MemoryStream(data))
            {
                using (var output = new MemoryStream())
                {
                    using (var image = new ImageFactory(preserveExifData: true))
                    {
                        image.Load(input)
                            .Resize(size)
                            .Format(format)//Make Dynamic
                            .Save(output);
                        ;
                    }

                    resized = output.ToArray();
                }
            }

            return resized;
        }
        #endregion
    }
}