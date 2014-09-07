namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using King.Azure.Data;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImageController : ImageApiController
    {
        public ImageController()
            : base("UseDevelopmentStorage=true")
        {
        }
    }
}