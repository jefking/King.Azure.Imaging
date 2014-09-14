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
                Width = 100,
            };
            versions.Add("Thumb", thumb);

            var medium = new ImageVersion()
            {
                Width = 400,
            };
            versions.Add("Medium", medium);

            var large = new ImageVersion()
            {
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