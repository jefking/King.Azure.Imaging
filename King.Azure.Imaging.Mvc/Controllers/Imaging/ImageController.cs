namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using Microsoft.Azure;
    
    public class ImageController : ImageApiController
    {
        private static readonly string connection = CloudConfigurationManager.GetSetting("StorageAccount");

        public ImageController()
            : base(connection)
        {
        }
    }
}