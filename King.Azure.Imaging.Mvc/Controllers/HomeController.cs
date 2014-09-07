namespace King.Azure.Imaging.Mvc.Controllers
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        #region Members
        /// <summary>
        /// Storage Elements
        /// </summary>
        private static readonly IStorageElements elements = new StorageElements();
        #endregion

        #region Methods
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Images()
        {
            var table = new TableStorage(elements.Table, "UseDevelopmentStorage=true");
            var data = from t in table.QueryByRow<ImageEntity>("thumb")
                   select t;

            return View(data);
        }
        #endregion
    }
}