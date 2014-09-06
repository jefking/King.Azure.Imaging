namespace King.Azure.Imaging.Mvc.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        #region Methods
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}