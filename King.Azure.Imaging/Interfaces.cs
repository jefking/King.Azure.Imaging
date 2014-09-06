namespace King.Azure.Imaging
{
    using System.Threading.Tasks;

    public interface IImagePreprocessor
    {
        #region Methods
        Task Process(byte[] content, string contentType, string fileName);
        #endregion
    }

    public interface IStorageElements
    {
        string Container
        {
            get;
        }
        string Queue
        {
            get;
        }
        string Table
        {
            get;
        }
    }
}