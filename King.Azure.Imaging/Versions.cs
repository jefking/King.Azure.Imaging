namespace King.Azure.Imaging
{
    using ImageProcessor.Imaging.Formats;
    using System.Collections.Generic;

    /// <summary>
    /// Versions
    /// </summary>
    public class Versions : IVersions
    {
        #region Members
        /// <summary>
        /// Versions
        /// </summary>
        protected static readonly IDictionary<string, IImageVersion> versions = new Dictionary<string, IImageVersion>(3);
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        static Versions()
        {
            var thumb = new ImageVersion
            {
                Width = 100,
                Format = new GifFormat { Quality = 50 },
            };
            versions.Add("thumb", thumb);

            var medium = new ImageVersion
            {
                Width = 640,
                Format = new JpegFormat { Quality = 70 },
            };
            versions.Add("medium", medium);

            var large = new ImageVersion
            {
                Width = 1080,
                Format = new JpegFormat { Quality = 85 },
            };
            versions.Add("large", large);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Image Versions
        /// </summary>
        public virtual IDictionary<string, IImageVersion> Images
        {
            get
            {
                return versions;
            }
        }
        #endregion
    }
}