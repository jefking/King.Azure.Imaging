namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using Microsoft.Azure;

    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImageController : ImageApiController
    {
        private static readonly string connection = CloudConfigurationManager.GetSetting("StorageAccount");

        public ImageController()
            : base(connection)
        {
        }
    }
}