namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System.Configuration;

    /// <summary>
    /// Data Controller
    /// </summary>
    public class DataController : ImageDataApiController
    {
        private static readonly string connection = ConfigurationManager.AppSettings["StorageAccount"];

        public DataController()
            : base(connection)
        {
        }
    }
}