namespace King.Azure.Imaging
{
    using ImageProcessor.Imaging.Formats;
    using King.Azure.Imaging.Models;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;

    #region IPreprocessor
    /// <summary>
    /// Image Preprocessor Interface
    /// </summary>
    public interface IPreprocessor
    {
        #region Methods
        /// <summary>
        /// Preprocess uploaded image
        /// </summary>
        /// <param name="content">Content</param>
        /// <param name="contentType">Content Type</param>
        /// <param name="fileName">File Name</param>
        /// <returns>Identifier</returns>
        Task<Guid> Process(byte[] content, string contentType, string fileName);
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
    /// <summary>
    /// Image Versions Interface
    /// </summary>
    public interface IVersions
    {
        #region Methods
        /// <summary>
        /// Image Versions to Generate
        /// </summary>
        IDictionary<string, IImageVersion> Images
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IStreamer
    /// <summary>
    /// Content Streamer
    /// </summary>
    public interface IStreamer
    {
        #region Methods
        /// <summary>
        /// Get File as Stream
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Data Stream</returns>
        Task<Stream> Get(string file);

        /// <summary>
        /// Get File as Bytes[]
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Content</returns>
        Task<byte[]> GetBytes(string file);
        #endregion

        #region Properties
        /// <summary>
        /// Content Type
        /// </summary>
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
        ISupportedImageFormat Get(string extension = Naming.DefaultExtension, int quality = 90);
        #endregion
    }
    #endregion

    #region IImageStore
    /// <summary>
    /// Image Store
    /// </summary>
    public interface IDataStore
    {
        #region Properties
        /// <summary>
        /// Image Streamer
        /// </summary>
        IStreamer Streamer
        {
            get;
        }
        #endregion

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

        /// <summary>
        /// Resize
        /// </summary>
        /// <param name="file">File</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="format">Format</param>
        /// <param name="quality">Quality</param>
        /// <param name="cache">Cache</param>
        /// <returns></returns>
        Task<ImageData> Resize(string file, int width, int height = 0, string format = Naming.DefaultExtension, int quality = Imaging.DefaultImageQuality, bool cache = true);
        #endregion
    }
    #endregion

    #region INaming
    /// <summary>
    /// File Naming Interface
    /// </summary>
    public interface INaming
    {
        #region Methods
        /// <summary>
        /// Naming for dynamic versions, enforce file uniqueness
        /// </summary>
        /// <param name="extension">Extension</param>
        /// <param name="quality">Quality</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Name</returns>
        string DynamicVersion(string extension, int quality, int width, int height);

        /// <summary>
        /// File name
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="version">Version</param>
        /// <param name="extension">Extension</param>
        /// <returns>Name</returns>
        string FileName(Guid identifier, string version, string extension);

        /// <summary>
        /// Original File Name
        /// </summary>
        /// <param name="data">Image Queued</param>
        /// <returns>Original File Name</returns>
        string OriginalFileName(ImageQueued data);

        /// <summary>
        /// File Name for Image Queued
        /// </summary>
        /// <param name="data">Image Queued</param>
        /// <param name="key">Version Key</param>
        /// <param name="extension">Extension</param>
        /// <returns>File Name</returns>
        string FileName(ImageQueued data, string key, string extension);

        /// <summary>
        /// Partial File Name
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <returns>Partial Name Format</returns>
        string FileNamePartial(Guid identifier);

        /// <summary>
        /// Identifier From File Name
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>Identifier</returns>
        Guid FromFileName(string file);

        /// <summary>
        /// Extension
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>Extension</returns>
        string Extension(string file);

        /// <summary>
        /// Relative Path
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <param name="file">File</param>
        /// <returns>Relative Path</returns>
        string RelativePath(string folder, string file);
        #endregion
    }
    #endregion
}