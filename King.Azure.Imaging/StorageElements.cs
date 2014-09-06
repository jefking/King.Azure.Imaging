namespace King.Azure.Imaging
{
    /// <summary>
    /// Storage Elements
    /// </summary>
    public class StorageElements : IStorageElements
    {
        #region Properties
        public string Container
        {
            get
            {
                return "images";
            }
        }
        public string Queue
        {
            get
            {
                return "imaging";
            }
        }
        public string Table
        {
            get
            {
                return "imaging";
            }
        }
        #endregion
    }
}