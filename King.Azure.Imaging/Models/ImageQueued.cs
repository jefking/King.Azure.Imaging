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
        public string FileNameFormat
        {
            get;
            set;
        }

        /// <summary>
        /// Original File Extension
        /// </summary>
        public string OriginalExtension
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Identifier
        {
            get;
            set;
        }
        #endregion
    }
}