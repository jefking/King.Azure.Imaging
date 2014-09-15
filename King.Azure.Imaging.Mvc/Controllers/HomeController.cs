namespace King.Azure.Imaging.Mvc.Controllers
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using System.Configuration;
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

        /// <summary>
        /// Connection String
        /// </summary>
        private static readonly string connection = ConfigurationManager.AppSettings["StorageAccount"];
        #endregion

        #region Methods
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Thumbs()
        {
            var table = new TableStorage(elements.Table, connection);
            var data = table.QueryByRow<ImageEntity>("thumb");

            return View(data);
        }

        public ActionResult Originals()
        {
            var table = new TableStorage(elements.Table, connection);
            var data = table.QueryByRow<ImageEntity>(ImagePreprocessor.Original);

            return View(data);
        }

        public ActionResult Large()
        {
            var table = new TableStorage(elements.Table, connection);
            var data = table.QueryByRow<ImageEntity>("large");

            return View(data);
        }

        public ActionResult Medium()
        {
            var table = new TableStorage(elements.Table, connection);
            var data = table.QueryByRow<ImageEntity>("medium");

            return View(data);
        }

        public ActionResult Dynamic()
        {
            var table = new TableStorage(elements.Table, connection);
            var data = table.QueryByRow<ImageEntity>(ImagePreprocessor.Original);

            return View(data);
        }
        #endregion
    }
}