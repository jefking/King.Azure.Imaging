namespace King.Azure.Imaging.Models
{
    /// <summary>
    /// Image Data
    /// </summary>
    public class ImageData
    {
        #region Properties
        /// <summary>
        /// Raw Data
        /// </summary>
        public byte[] Raw
        {
            get;
            set;
        }

        /// <summary>
        /// Mime Type
        /// </summary>
        public string MimeType
        {
            get;
            set;
        }
        #endregion
    }
}