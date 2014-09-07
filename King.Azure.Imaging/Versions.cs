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
        protected readonly IDictionary<string, string> versions = new Dictionary<string, string>(3);
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Versions()
        {
            versions.Add("Thumb", "width=100&height=100&crop=auto&format=jpg");
            versions.Add("Medium", "maxwidth=400&maxheight=400&format=jpg");
            versions.Add("Large", "maxwidth=1900&maxheight=1900&format=jpg");
        }
        #endregion

        #region Properties
        /// <summary>
        /// Image Versions
        /// </summary>
        public virtual IDictionary<string, string> Images
        {
            get
            {
                return this.versions;
            }
        }
        #endregion
    }
}