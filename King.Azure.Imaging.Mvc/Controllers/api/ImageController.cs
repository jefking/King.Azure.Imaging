namespace King.Azure.Imaging.Mvc.Controllers.api
{
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