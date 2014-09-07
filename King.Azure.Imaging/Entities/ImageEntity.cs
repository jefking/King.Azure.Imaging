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
        /// Content Type
        /// </summary>
        public string ContentType
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
        #endregion
    }
}