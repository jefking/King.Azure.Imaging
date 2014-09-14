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
        protected readonly IDictionary<string, IImageVersion> versions = new Dictionary<string, IImageVersion>(3);
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Versions()
        {
            var thumb = new ImageVersion
            {
                Width = 100,
                Format = new JpegFormat { Quality = 50 },
            };
            versions.Add("Thumb", thumb);

            var medium = new ImageVersion
            {
                Width = 640,
                Format = new JpegFormat { Quality = 70 },
            };
            versions.Add("Medium", medium);

            var large = new ImageVersion
            {
                Width = 1080,
                Format = new JpegFormat { Quality = 85 },
            };
            versions.Add("Large", large);
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
                return this.versions;
            }
        }
        #endregion
    }
}