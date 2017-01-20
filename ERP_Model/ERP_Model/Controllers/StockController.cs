using System.Web.Mvc;

namespace ERP_Model.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockItems()
        {
            return View();
        }

        public ActionResult StockTransactions()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }
    }
}