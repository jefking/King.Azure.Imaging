namespace King.Azure.Imaging
{
    using System.Threading.Tasks;

    #region IImagePreprocessor
    /// <summary>
    /// Image Preprocessor Interface
    /// </summary>
    public interface IImagePreprocessor
    {
        #region Methods
        /// <summary>
        /// Preprocess uploaded image
        /// </summary>
        /// <param name="content">Content</param>
        /// <param name="contentType">Content Type</param>
        /// <param name="fileName">File Name</param>
        /// <returns>Task</returns>
        Task Process(byte[] content, string contentType, string fileName);
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