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
        /// <param name="mimeType">Mime Type</param>
        /// <param name="fileName">File Name</param>
        /// <returns>Identifier</returns>
        public virtual async Task<Guid> Process(byte[] content, string mimeType, string fileName)
        {
            if (null == content || !content.Any())
            {
                throw new ArgumentException("content");
            }
            if (string.IsNullOrWhiteSpace(mimeType))
            {
                throw new ArgumentException("mimeType");
            }
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("fileName");
            }

            var id = Guid.NewGuid();
            var extension = this.naming.Extension(fileName);
            var originalFileName = this.naming.FileName(id, Naming.Original, extension);

            await this.store.Save(originalFileName, content, Naming.Original, mimeType, id, true, extension, 100);

            return id;
        }
        #endregion
    }
}