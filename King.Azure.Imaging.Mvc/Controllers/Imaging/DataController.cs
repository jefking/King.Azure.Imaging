namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System.Configuration;

    public class DataController : ImageDataApiController
    {
        private static readonly string connection = CloudConfigurationManager.GetSetting("StorageAccount");

        public DataController()
            : base(connection)
        {
        }
    }
}