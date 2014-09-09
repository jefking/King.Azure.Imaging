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

        public ActionResult Thumbs()
        {
            var table = new TableStorage(elements.Table, "UseDevelopmentStorage=true");
            var data = from t in table.QueryByRow<ImageEntity>("thumb")
                       select t;

            return View(data);
        }

        public ActionResult Originals()
        {
            var table = new TableStorage(elements.Table, "UseDevelopmentStorage=true");
            var data = from t in table.QueryByRow<ImageEntity>("original")
                       select t;

            return View(data);
        }

        public ActionResult Large()
        {
            var table = new TableStorage(elements.Table, "UseDevelopmentStorage=true");
            var data = from t in table.QueryByRow<ImageEntity>("large")
                       select t;

            return View(data);
        }

        public ActionResult Medium()
        {
            var table = new TableStorage(elements.Table, "UseDevelopmentStorage=true");
            var data = from t in table.QueryByRow<ImageEntity>("medium")
                       select t;

            return View(data);
        }

        public ActionResult Dynamic()
        {
            var table = new TableStorage(elements.Table, "UseDevelopmentStorage=true");
            var data = from t in table.QueryByRow<ImageEntity>("original")
                       select t;

            return View(data);
        }
        #endregion
    }
}