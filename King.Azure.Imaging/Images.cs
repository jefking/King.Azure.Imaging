namespace King.Azure.Imaging
{
    using King.Azure.Imaging.Models;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    /// <summary>
    /// Images
    /// </summary>
    public class Images
    {
        #region Methods
        /// <summary>
        /// Resize Image that is a Square
        /// </summary>
        /// <param name="stream">File Stream</param>
        /// <param name="format">Image Format</param>
        /// <param name="maxSize">Max Size</param>
        /// <returns>Image Bytes</returns>
        public virtual byte[] ResizeSquare(Stream stream, ImageFormat format, int maxSize)
        {
            if (null == stream)
            {
                throw new ArgumentNullException("stream");
            }
            else if (null == format)
            {
                throw new ArgumentNullException("format");
            }
            else if (0 > maxSize)
            {
                throw new ArgumentException("maxSize cannot be less than zero");
            }

            using (var image = Image.FromStream(stream))
            {
                var crop = image.Width > maxSize;
                var size = new Size()
                {
                    Width = maxSize,
                    Height = maxSize,
                };

                var rec = new RectangleF()
                {
                    Size = image.Size,
                    X = 0,
                    Y = 0,
                };

                return this.Crop(image, format, size, rec);
            }
        }

        /// <summary>
        /// Resize User Profile Image
        /// </summary>
        /// <param name="stream">Can be streamed from a URL or streamed via a form-posted image</param>
        /// <param name="savingFormat">Format - see helper functions for options</param>
        /// <param name="message">An output message in case of an error.</param>
        /// <returns>Image Data</returns>
        public virtual ImageData ResizeByWidth(Stream stream, ImageFormat format, int maxWidth)
        {
            if (null == stream)
            {
                throw new ArgumentNullException("stream");
            }
            if (null == format)
            {
                throw new ArgumentNullException("format");
            }
            if (0 > maxWidth)
            {
                throw new ArgumentException("maxWidth cannot be less than zero");
            }

            using (var image = Image.FromStream(stream))
            {
                var sourceWidth = image.Width;
                var crop = sourceWidth > maxWidth;
                var setHeight = (int)image.Height * maxWidth / sourceWidth;
                var size = new Size()
                {
                    Height = setHeight,
                    Width = maxWidth,
                };

                var rec = new RectangleF()
                {
                    Size = image.Size,
                    X = 0,
                    Y = 0,
                };

                return new ImageData()
                {
                    Size = size,
                    Data = this.Crop(image, format, size, rec),
                };
            }
        }

        /// <summary>
        /// Crop Image
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="format">Image Format</param>
        /// <param name="size">Size</param>
        /// <param name="dimensions">Dimensions</param>
        /// <returns>Image Bytes</returns>
        public virtual byte[] Crop(Image image, ImageFormat format, Size size, RectangleF dimensions)
        {
            if (null == image)
            {
                throw new ArgumentNullException("image");
            }
            if (null == format)
            {
                throw new ArgumentNullException("format");
            }
            if (0 > dimensions.X)
            {
                throw new ArgumentException("X cannot be less than zero");
            }
            if (0 > dimensions.Y)
            {
                throw new ArgumentException("Y cannot be less than zero");
            }

            using (var bitmap = new Bitmap(image))
            {
                using (var clone = bitmap.Clone(dimensions, PixelFormat.DontCare))
                {
                    using (var thumbnail = clone.GetThumbnailImage(size.Width, size.Height, null, IntPtr.Zero))
                    {
                        using (var ms = new MemoryStream())
                        {
                            thumbnail.Save(ms, format);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
        }
        #endregion
    }
}