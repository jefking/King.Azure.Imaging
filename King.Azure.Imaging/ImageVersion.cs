using ImageProcessor.Imaging.Formats;
namespace King.Azure.Imaging
{
    public class ImageVersion : IImageVersion
    {
        #region Properties
        /// <summary>
        /// Image Width
        /// </summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Image Height
        /// </summary>
        public int Height
        {
            get;
            set;
        }

        /// <summary>
        /// Image Format
        /// </summary>
        public ISupportedImageFormat Format
        {
            get;
            set;
        }
        #endregion
    }
}