namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System.Configuration;

    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImageController : ImageApiController
    {
        private static readonly string connection = ConfigurationManager.AppSettings["StorageAccount"];

        public ImageController()
            : base(connection)
        {
        }
    }
}