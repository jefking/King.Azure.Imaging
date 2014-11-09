namespace King.Azure.Imaging.Models
{
    #region ITaskConfiguration
    /// <summary>
    /// Task Configuration Interface
    /// </summary>
    public interface ITaskConfiguration
    {
        #region Properties
        /// <summary>
        /// Storage Elements
        /// </summary>
        IStorageElements StorageElements
        {
            get;
        }

        /// <summary>
        /// Image Versions
        /// </summary>
        IVersions Versions
        {
            get;
        }

        /// <summary>
        /// Storage Connection String
        /// </summary>
        string ConnectionString
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IStorageElements
    /// <summary>
    /// Storage Elements Interface
    /// </summary>
    public interface IStorageElements
    {
        #region Properties
        /// <summary>
        /// Container to store images
        /// </summary>
        string Container
        {
            get;
        }

        /// <summary>
        /// Queue to save tasks to
        /// </summary>
        string Queue
        {
            get;
        }

        /// <summary>
        /// Table for storing image meta-data
        /// </summary>
        string Table
        {
            get;
        }
        #endregion
    }
    #endregion
}