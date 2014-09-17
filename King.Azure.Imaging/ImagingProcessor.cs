namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Imaging Task
    /// </summary>
    public class ImagingProcessor : IProcessor<ImageQueued>
    {
        #region Members
        /// <summary>
        /// Versions
        /// </summary>
        protected readonly IDictionary<string, IImageVersion> versions = null;

        /// <summary>
        /// Container
        /// </summary>
        protected readonly IContainer container = null;

        /// <summary>
        /// Image Store
        /// </summary>
        protected readonly IImageStore store = null;

        /// <summary>
        /// Imaging
        /// </summary>
        protected readonly IImaging imaging = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        public ImagingProcessor(IImaging imaging, IContainer container, IImageStore store, IDictionary<string, IImageVersion> versions)
        {
            if (null == imaging)
            {
                throw new ArgumentNullException("imaging");
            }
            if (null == container)
            {
                throw new ArgumentNullException("container");
            }
            if (null == store)
            {
                throw new ArgumentNullException("store");
            }
            if (null == versions)
            {
                throw new ArgumentNullException("versions");
            }

            this.imaging = imaging;
            this.container = container;
            this.store = store;
            this.versions = versions;
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
            var original = string.Format(data.FileNameFormat, ImagePreprocessor.Original, data.OriginalExtension).ToLowerInvariant();

            var bytes = await container.Get(original);
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