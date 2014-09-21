namespace King.Azure.Imaging
{
    /// <summary>
    /// Storage Elements
    /// </summary>
    public class StorageElements : IStorageElements
    {
        #region Properties
        /// <summary>
        /// Container to store images
        /// </summary>
        public virtual string Container
        {
            get
            {
                const string name = "images";
                return name;
            }
        }

        /// <summary>
        /// Queue to save tasks to
        /// </summary>
        public virtual string Queue
        {
            get
            {
                const string name = "imaging";
                return name;
            }
        }

        /// <summary>
        /// Table for storing image meta-data
        /// </summary>
        public virtual string Table
        {
            get
            {
                const string name = "imaging";
                return name;
            }
        }
        #endregion
    }
}