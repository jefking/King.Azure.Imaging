namespace King.Azure.Imaging
{
    using ImageProcessor.Imaging.Formats;
    using System.Linq;

    /// <summary>
    /// Image Format Factory
    /// </summary>
    public class ImageFormatFactory
    {
        #region Methods
        public ISupportedImageFormat Get(string extension)
        {
            var formats = new ISupportedImageFormat[] { new BitmapFormat(), new GifFormat(), new JpegFormat(), new PngFormat(), new TiffFormat() };
            foreach(var format in formats)
            {
                var f = (from e in format.FileExtensions
                         where extension == e
                         select e).FirstOrDefault();

                if (null != f)
                {
                    return format;
                }
            }

            return null;
        }
        #endregion
    }
}