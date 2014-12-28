namespace King.Azure.Imaging.Mvc.Controllers
{
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using System.Configuration;
    using System.Threading.Tasks;
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

        /// <summary>
        /// Table Storage (Image Meta-Data)
        /// </summary>
        private readonly ITableStorage table = new TableStorage(elements.Table, connection);
        #endregion

        #region Methods
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Thumbs()
        {
            var data = await this.table.QueryByRow<ImageEntity>("thumb");

            return View(data);
        }

        public async Task<ActionResult> Originals()
        {
            var data = await this.table.QueryByRow<ImageEntity>(Naming.Original);

            return View(data);
        }

        public async Task<ActionResult> Large()
        {
            var data = await this.table.QueryByRow<ImageEntity>("large");

            return View(data);
        }

        public async Task<ActionResult> Medium()
        {
            var data = await this.table.QueryByRow<ImageEntity>("medium");

            return View(data);
        }

        public async Task<ActionResult> Dynamic()
        {
            var data = await this.table.QueryByRow<ImageEntity>(Naming.Original);

            return View(data);
        }
        #endregion
    }
}