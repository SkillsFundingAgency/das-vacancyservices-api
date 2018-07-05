using System.Web.Mvc;

namespace Esfa.Vacancy.Manage.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Api", "Home");
        }

        public ActionResult Api()
        {
            return View();
        }
    }
}
