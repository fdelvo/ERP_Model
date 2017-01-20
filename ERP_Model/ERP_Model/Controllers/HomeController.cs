using System.Web.Mvc;

namespace ERP_Model.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}