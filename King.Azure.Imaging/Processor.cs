namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Imaging Task Processor
    /// </summary>
    public class Processor : IProcessor<ImageQueued>
    {
        #region Members
        /// <summary>
        /// Versions
        /// </summary>
        protected readonly IReadOnlyDictionary<string, IImageVersion> versions = null;

        /// <summary>
        /// Image Store
        /// </summary>
        protected readonly IDataStore store = null;

        /// <summary>
        /// Imaging
        /// </summary>
        protected readonly IImaging imaging = null;

        /// <summary>
        /// Naming
        /// </summary>
        protected readonly INaming naming = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Processor(IDataStore store, IReadOnlyDictionary<string, IImageVersion> versions)
            : this(store, versions, new Imaging(), new Naming())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Processor(IDataStore store, IReadOnlyDictionary<string, IImageVersion> versions, IImaging imaging, INaming naming)
        {
            if (null == store)
            {
                throw new ArgumentNullException("store");
            }
            if (null == versions)
            {
                throw new ArgumentNullException("versions");
            }
            if (null == imaging)
            {
                throw new ArgumentNullException("imaging");
            }
            if (null == naming)
            {
                throw new ArgumentNullException("naming");
            }

            this.store = store;
            this.versions = versions;
            this.imaging = imaging;
            this.naming = naming;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process Image Queued
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Successful</returns>
        public virtual async Task<bool> Process(ImageQueued data)
        {
            var original = this.naming.OriginalFileName(data);

            var streamer = this.store.Streamer;
            var bytes = await streamer.Bytes(original);
            foreach (var key in this.versions.Keys)
            {
                var version = this.versions[key];
                var filename = this.naming.FileName(data, key, version.Format.DefaultExtension);

                var resized = this.imaging.Resize(bytes, version);

                await this.store.Save(filename, resized, key, version.Format.MimeType, data.Identifier, false, null, (byte)version.Format.Quality);
            }

            return true;
        }
        #endregion
    }
}