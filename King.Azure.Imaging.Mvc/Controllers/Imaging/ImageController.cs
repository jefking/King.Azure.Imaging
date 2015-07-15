namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using Microsoft.Azure;

    //[Authorize(Roles = "Users")] Uncomment to secure controller
    public class ImageController : ImageApiController
    {
        private static readonly string connection = CloudConfigurationManager.GetSetting("StorageAccount");

        public ImageController()
            : base(connection)
        {
        }

        /*
        /// Uncomment to secure method
        [HttpPost]
        [Authorize(Roles = "Users")]
        public override async Task<HttpResponseMessage> Post()
        {
            return await base.Post();
        }
        
        /// Uncomment to secure method
        [HttpGet]
        [Authorize(Roles = "Users")]
        public override async Task<HttpResponseMessage> Get(string file, ushort width = 0, ushort height = 0, string format = Naming.DefaultExtension, byte quality = Imaging.DefaultImageQuality, bool cache = true)
        {
            return await base.Get(file, width, height, format, quality, cache);
        }
        */
    }
}