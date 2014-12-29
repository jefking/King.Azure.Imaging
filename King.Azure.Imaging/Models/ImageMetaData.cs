namespace King.Azure.Imaging.Models
{
    using King.Azure.Data;
    using King.Mapper;
    using System;

    /// <summary>
    /// Image Meta Data
    /// </summary>
    public class ImageMetaData
    {
        #region Properties
        /// <summary>
        /// Identifier
        /// </summary>
        [ActionName(TableStorage.PartitionKey)]
        public virtual Guid Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ActionName(TableStorage.RowKey)]
        public virtual string Version
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ActionName(TableStorage.Timestamp)]
        public virtual string CreatedOn
        {
            get;
            set;
        }
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
        public virtual uint FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// Image Width
        /// </summary>
        public virtual ushort Width
        {
            get;
            set;
        }

        /// <summary>
        /// Image Height
        /// </summary>
        public virtual ushort Height
        {
            get;
            set;
        }

        /// <summary>
        /// Image Quality
        /// </summary>
        public virtual byte Quality
        {
            get;
            set;
        }
        #endregion
    }
}