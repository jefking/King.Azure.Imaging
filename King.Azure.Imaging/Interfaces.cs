namespace King.Azure.Imaging
{
    using ImageProcessor.Imaging.Formats;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
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
        #region Methods
        Task<Stream> Get(string file);
        Task<byte[]> GetBytes(string file);
        #endregion

        #region Properties
        string ContentType
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IImageVersion
    /// <summary>
    /// Image Version to be generated automatically
    /// </summary>
    /// <remarks>
    /// Specify either Width or Height you don't need to have both.
    /// </remarks>
    public interface IImageVersion
    {
        #region Properties
        /// <summary>
        /// Image Width
        /// </summary>
        int Width
        {
            get;
        }

        /// <summary>
        /// Image Height
        /// </summary>
        int Height
        {
            get;
        }

        /// <summary>
        /// Image Format
        /// </summary>
        ISupportedImageFormat Format
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IImaging
    /// <summary>
    /// Imaging Interface
    /// </summary>
    public interface IImaging
    {
        #region Methods
        /// <summary>
        /// Dimensions of Image
        /// </summary>
        /// <param name="data">Image Bytes</param>
        /// <returns>Image Size</returns>
        Size Size(byte[] data);

        /// <summary>
        /// Resize Image
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="version">Version</param>
        /// <returns>Image Bytes</returns>
        byte[] Resize(byte[] data, IImageVersion version);

        /// <summary>
        /// Get Image Format
        /// </summary>
        /// <param name="extension">Extension</param>
        /// <returns>Image Format</returns>
        ISupportedImageFormat Get(string extension = ImagePreprocessor.DefaultExtension, int quality = 100);
        #endregion
    }
    #endregion

    #region IImageStore
    public interface IImageStore
    {
        #region Methods
        /// <summary>
        /// Save to data stores
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="content">Content</param>
        /// <param name="version">Version</param>
        /// <param name="mimeType">MimeType/Content Type</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="queueForResize">Queue for Resize</param>
        /// <param name="extension">Extension</param>
        /// <param name="quality">Quality</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        Task Save(string fileName, byte[] content, string version, string mimeType, Guid identifier, bool queueForResize = false, string extension = null, int quality = 100, int width = 0, int height = 0);
        #endregion
    }
    #endregion
}