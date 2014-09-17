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
        public virtual int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Image Height
        /// </summary>
        public virtual int Height
        {
            get;
            set;
        }

        /// <summary>
        /// Image Format
        /// </summary>
        public virtual ISupportedImageFormat Format
        {
            get;
            set;
        }
        #endregion
    }
}