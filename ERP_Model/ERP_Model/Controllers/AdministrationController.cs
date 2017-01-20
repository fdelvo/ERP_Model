using System.Web.Mvc;

namespace ERP_Model.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewAddress()
        {
            return View();
        }

        public ActionResult EditAddress()
        {
            return View();
        }

        public ActionResult NewUser()
        {
            return View();
        }

        public ActionResult EditUser()
        {
            return View();
        }
    }
}