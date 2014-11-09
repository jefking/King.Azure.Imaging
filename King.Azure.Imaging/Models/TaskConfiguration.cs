namespace King.Azure.Imaging.Models
{
    /// <summary>
    /// Task Configuration
    /// </summary>
    public class TaskConfiguration : ITaskConfiguration
    {
        #region Properties
        /// <summary>
        /// Storage Elements
        /// </summary>
        public virtual IStorageElements StorageElements
        {
            get;
            set;
        }

        /// <summary>
        /// Image Versions
        /// </summary>
        public virtual IVersions Versions
        {
            get;
            set;
        }

        /// <summary>
        /// Storage Connection String
        /// </summary>
        public virtual string ConnectionString
        {
            get;
            set;
        }
        #endregion
    }
}