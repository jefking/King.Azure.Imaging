namespace King.Azure.Imaging
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Preprocessor
    /// </summary>
    public class ImagePreprocessor : IImagePreprocessor
    {
        #region Members
        /// <summary>
        /// Original
        /// </summary>
        public const string Original = "original";

        /// <summary>
        /// Default Extension
        /// </summary>
        public const string DefaultExtension = "jpeg";

        /// <summary>
        /// File Name Format
        /// </summary>
        public const string FileNameFormat = "{0}_{1}.{2}";

        /// <summary>
        /// Path Format
        /// </summary>
        public const string PathFormat = "{0}/{1}";

        /// <summary>
        /// Image Store
        /// </summary>
        protected readonly IImageStore store = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public ImagePreprocessor(string connectionString)
            : this(new ImageStore(connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImagePreprocessor(IImageStore store)
        {
            if (null == store)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Preprocess uploaded image
        /// </summary>
        /// <param name="content">Content</param>
        /// <param name="contentType">Content Type</param>
        /// <param name="fileName">File Name</param>
        /// <returns>Task</returns>
        public virtual async Task Process(byte[] content, string contentType, string fileName)
        {
            if (null == content || !content.Any())
            {
                throw new ArgumentNullException("content");
            }
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException("contentType");
            }
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("fileName");
            }

            var id = Guid.NewGuid();
            var extension = fileName.Contains('.') ? fileName.Substring(fileName.LastIndexOf('.') + 1) : ImagePreprocessor.DefaultExtension;
            var originalFileName = string.Format(FileNameFormat, id, Original, extension);

            await this.store.Save(originalFileName, content, Original, contentType, id, true, extension);
        }
        #endregion
    }
}