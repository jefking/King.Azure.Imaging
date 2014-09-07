namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Imaging Task
    /// </summary>
    public class ImagingProcessor : IProcessor<ImageQueued>
    {
        #region Methods
        public Task<bool> Process(ImageQueued data)
        {
            throw new System.NotImplementedException();
        }
        #endregion

    }
}