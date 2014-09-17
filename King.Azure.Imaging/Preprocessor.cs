namespace King.Azure.Imaging
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Preprocessor
    /// </summary>
    public class Preprocessor : IPreprocessor
    {
        #region Members
        /// <summary>
        /// Image Store
        /// </summary>
        protected readonly IDataStore store = null;

        /// <summary>
        /// Image Naming
        /// </summary>
        protected readonly INaming naming = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public Preprocessor(string connectionString)
            : this(new DataStore(connectionString), new Naming())
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public Preprocessor(IDataStore store, INaming naming)
        {
            if (null == store)
            {
                throw new ArgumentNullException("store");
            }
            if (null == naming)
            {
                throw new ArgumentNullException("naming");
            }

            this.store = store;
            this.naming = naming;
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
            var extension = this.naming.Extension(fileName);
            var originalFileName = this.naming.FileName(id, Naming.Original, extension);

            await this.store.Save(originalFileName, content, Naming.Original, contentType, id, true, extension);
        }
        #endregion
    }
}