namespace King.Azure.Imaging.Models
{
    using System;

    /// <summary>
    /// Image Queued
    /// </summary>
    public class ImageQueued
    {
        #region Properties
        /// <summary>
        /// File Name Format
        /// </summary>
        public virtual string FileNameFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Original File Extension
        /// </summary>
        public virtual string OriginalExtension
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier
        /// </summary>
        public virtual Guid Identifier
        {
            get;
            set;
        }
        #endregion
    }
}