namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Azure;

    //[Authorize(Roles = "Users")] Uncomment to secure controller
    public class DataController : ImageDataApiController
    {
        private static readonly string connection = CloudConfigurationManager.GetSetting("StorageAccount");

        public DataController()
            : base(connection)
        {
        }

        /*
        /// Uncomment to secure method
        [HttpGet]
        [Authorize(Roles = "Users")]
        public override async Task<HttpResponseMessage> Get(Guid? id = default(Guid?), string version = null, string file = null)
        {
            return await base.Get(id, version, file);
        }
        */
    }
}