namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using King.Mapper;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;

    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImageController : ApiController
    {
        #region Members
        /// <summary>
        /// Image Preprocessor
        /// </summary>
        private readonly IImagePreProcessor preprocessor = new ImagePreProcessor("connection string");
        #endregion

        #region Methods
        public async Task UploadImage()
        {
            var bytes = await Request.Content.ReadAsByteArrayAsync();
            var contentType = Request.Headers.GetValues(ImagePreProcessor.ContentTypeHeader).FirstOrDefault();
            var fileName = Request.Headers.GetValues(ImagePreProcessor.FileNameHeader).FirstOrDefault();
            await this.preprocessor.Process(bytes, contentType, fileName);
        }
        #endregion
    }
}