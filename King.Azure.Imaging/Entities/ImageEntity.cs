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
        public virtual string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// File Name
        /// </summary>
        public virtual string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Relative Path
        /// </summary>
        public virtual string RelativePath
        {
            get;
            set;
        }

        /// <summary>
        /// File Size
        /// </summary>
        public virtual long FileSize
        {
            get;
            set;
        }

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
        /// Image Quality
        /// </summary>
        public virtual int Quality
        {
            get;
            set;
        }
        #endregion
    }
}