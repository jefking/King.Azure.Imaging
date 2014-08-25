namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using King.Azure.Imaging.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class ImageController : ApiController
    {
        private readonly Upload uploader = new Upload();

        public async Task UploadImage()
        {
            var image = uploader.Load(Request);
        }
    }
}