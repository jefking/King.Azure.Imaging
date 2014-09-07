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
        private static readonly IStorageElements elements = new StorageElements();
        private readonly ITableStorage table = new TableStorage(elements.Table, "UseDevelopmentStorage=true");

        #region Methods
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Images()
        {
            var data = from t in table.QueryByRow<ImageEntity>("thumb")
                   select t.FileName;

            return View(data);
        }
        #endregion
    }
}