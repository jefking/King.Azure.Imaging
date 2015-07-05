namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System.Configuration;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <summary>
    /// Secure Image Controller
    /// </summary>
    /// <remarks>
    /// This is an example of a way to secure the API to your needs, this may or may not work in your environment.
    /// - if you don't need it on the per API call, you can just decorate the class.
    /// </remarks>
    [Authorize(Roles = "Users")]
    public class SecureImageController : ImageApiController
    {
        private static readonly string connection = ConfigurationManager.AppSettings["StorageAccount"];

        public SecureImageController()
            : base(connection)
        {
        }

        [HttpPost]
        [Authorize(Roles = "Users")]
        public override async Task<HttpResponseMessage> Post()
        {
            return await base.Post();
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public override async Task<HttpResponseMessage> Get(string file, ushort width = 0, ushort height = 0, string format = Naming.DefaultExtension, byte quality = Imaging.DefaultImageQuality, bool cache = true)
        {
            return await base.Get(file, width, height, format, quality, cache);
        }
    }
}