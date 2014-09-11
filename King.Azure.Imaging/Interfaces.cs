namespace King.Azure.Imaging
{
    using System.Collections.Generic;
    using System.IO;
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

    #region IVersions
    public interface IVersions
    {
        #region Methods
        IDictionary<string, IImageVersion> Images
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IImageStreamer
    public interface IImageStreamer
    {
        Task<Stream> Get(string file);

        string ContentType
        {
            get;
        }
    }
    #endregion

    #region IImageVersion
    public interface IImageVersion
    {
        int Width
        {
            get;
            set;
        }
        int Height
        {
            get;
            set;
        }
    }
    #endregion
}