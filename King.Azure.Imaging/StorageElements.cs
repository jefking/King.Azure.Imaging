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
                return "images";
            }
        }

        /// <summary>
        /// Queue to save tasks to
        /// </summary>
        public virtual string Queue
        {
            get
            {
                return "imaging";
            }
        }

        /// <summary>
        /// Table for storing image meta-data
        /// </summary>
        public virtual string Table
        {
            get
            {
                return "imaging";
            }
        }
        #endregion
    }
}