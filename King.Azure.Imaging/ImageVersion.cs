namespace King.Azure.Imaging
{
    using ImageProcessor.Imaging.Formats;

    /// <summary>
    /// Image Version
    /// </summary>
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