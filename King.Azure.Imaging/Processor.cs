namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Imaging Task
    /// </summary>
    public class Processor : IProcessor<ImageQueued>
    {
        #region Members
        /// <summary>
        /// Versions
        /// </summary>
        protected readonly IDictionary<string, IImageVersion> versions = null;

        /// <summary>
        /// Image Store
        /// </summary>
        protected readonly IDataStore store = null;

        /// <summary>
        /// Imaging
        /// </summary>
        protected readonly IImaging imaging = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Processor(IDataStore store, IDictionary<string, IImageVersion> versions)
            : this(store, versions, new Imaging())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Processor(IDataStore store, IDictionary<string, IImageVersion> versions, IImaging imaging)
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

            this.store = store;
            this.versions = versions;
            this.imaging = imaging;
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
            var original = string.Format(data.FileNameFormat, Naming.Original, data.OriginalExtension).ToLowerInvariant();

            var streamer = this.store.Streamer;
            var bytes = await streamer.GetBytes(original);
            foreach (var key in this.versions.Keys)
            {
                var version = this.versions[key];
                var filename = string.Format(data.FileNameFormat, key, version.Format.DefaultExtension).ToLowerInvariant();

                var resized = this.imaging.Resize(bytes, version);

                await this.store.Save(filename, resized, key, version.Format.MimeType, data.Identifier, false, null, version.Format.Quality);
            }

            return true;
        }
        #endregion
    }
}