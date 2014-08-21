namespace King.Azure.Imaging.Models
{
    using System.Drawing;

    /// <summary>
    /// Image Data
    /// </summary>
    public class ImageData
    {
        #region Properties
        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// Size
        /// </summary>
        public Size Size
        {
            get;
            set;
        }
        #endregion
    }
}