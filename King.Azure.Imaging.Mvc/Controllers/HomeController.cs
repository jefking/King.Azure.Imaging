namespace King.Azure.Imaging.Mvc.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using King.Azure.Data;
    using King.Azure.Imaging.Entities;
    using King.Azure.Imaging.Models;
    using Microsoft.Azure;

    public class HomeController : Controller
    {
        private static readonly IStorageElements elements = new StorageElements();

        private static readonly string connection = CloudConfigurationManager.GetSetting("StorageAccount");

        private readonly ITableStorage table = new TableStorage(elements.Table, connection);

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
    }
}