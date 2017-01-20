using System.Web.Mvc;

namespace ERP_Model.Controllers
{
    public class SupplysController : Controller
    {
        // GET: Supplys
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SupplyItems()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}