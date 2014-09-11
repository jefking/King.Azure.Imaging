namespace King.Azure.Imaging
{
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
            var thumb = new ImageVersion()
            {
                Height = 100,
                Width = 100,
            };
            versions.Add("Thumb", thumb);

            var medium = new ImageVersion()
            {
                Height = 400,
                Width = 400,
            };
            versions.Add("Medium", medium);

            var large = new ImageVersion()
            {
                Height = 1900,
                Width = 1900,
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