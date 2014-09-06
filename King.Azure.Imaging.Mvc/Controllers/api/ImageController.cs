namespace King.Azure.Imaging.Mvc.Controllers.api
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Image Controller
    /// </summary>
    public class ImageController : ApiController
    {
        #region Members
        /// <summary>
        /// Image Preprocessor
        /// </summary>
        private readonly IImagePreprocessor preprocessor = new ImagePreprocessor("UseDevelopmentStorage=true");
        #endregion

        #region Methods
        public async Task Upload()
        {
            var bytes = await Request.Content.ReadAsByteArrayAsync();
            var contentType = Request.Headers.GetValues(ImagePreprocessor.ContentTypeHeader).FirstOrDefault();
            var fileName = Request.Headers.GetValues(ImagePreprocessor.FileNameHeader).FirstOrDefault();
            await this.preprocessor.Process(bytes, contentType, fileName);
        }
        #endregion
    }
}