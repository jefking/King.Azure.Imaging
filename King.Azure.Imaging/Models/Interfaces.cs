namespace King.Azure.Imaging.Models
{
    public interface ITaskConfiguration
    {
        IStorageElements StorageElements
        {
            get;
        }

        IVersions Versions
        {
            get;
        }

        string ConnectionString
        {
            get;
        }
    }

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