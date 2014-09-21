namespace King.Azure.Imaging.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Image Entity
    /// </summary>
    public class ImageEntity : TableEntity
    {
        #region Properties
        /// <summary>
        /// Mime Type
        /// </summary>
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Relative Path
        /// </summary>
        public string RelativePath
        {
            get;
            set;
        }

        /// <summary>
        /// File Size
        /// </summary>
        public long FileSize
        {
            get;
            set;
        }

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
        /// Image Quality
        /// </summary>
        public int Quality
        {
            get;
            set;
        }
        #endregion
    }
}