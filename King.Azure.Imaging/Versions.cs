namespace King.Azure.Imaging
{
    using ImageProcessor.Imaging.Formats;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Versions
    /// </summary>
    public class Versions : IVersions
    {
        #region Members
        /// <summary>
        /// Versions
        /// </summary>
        protected readonly IReadOnlyDictionary<string, IImageVersion> versions = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Versions()
        {
            var dictionary = new Dictionary<string, IImageVersion>(3);
            var thumb = new ImageVersion
            {
                Width = 100,
                Format = new GifFormat { Quality = 50 },
            };
            dictionary.Add("thumb", thumb);

            var medium = new ImageVersion
            {
                Width = 640,
                Format = new JpegFormat { Quality = 70 },
            };
            dictionary.Add("medium", medium);

            var large = new ImageVersion
            {
                Width = 1080,
                Format = new JpegFormat { Quality = 85 },
            };
            dictionary.Add("large", large);

            this.versions = new ReadOnlyDictionary<string, IImageVersion>(dictionary);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Image Versions
        /// </summary>
        public virtual IReadOnlyDictionary<string, IImageVersion> Images
        {
            get
            {
                return versions;
            }
        }
        #endregion
    }
}