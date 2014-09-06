namespace King.Azure.Imaging
{
    using King.Azure.Data;
    using King.Azure.Imaging.Models;
    using King.Service.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Imaging Task
    /// </summary>
    public class ImagingTask : Dequeue<ImageQueued>
    {
        #region Methods
        protected override async Task Process(IQueued<ImageQueued> message)
        {
            await base.Process(message);
        }
        #endregion
    }
}