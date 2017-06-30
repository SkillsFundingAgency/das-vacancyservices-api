using System;
using System.Text;
using System.Web.Mvc;
using Microsoft.Azure;

namespace Esfa.Vacancy.Register.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public ActionResult Index()
        {
            return RedirectToAction("Api", "Home");
        }

        public ActionResult Api()
        {
            return View();
        }

        [OutputCache(Duration = 86400)]
        public ContentResult RobotsText()
        {
            var builder = new StringBuilder();

            builder.AppendLine("User-agent: *");

            if (!bool.Parse(CloudConfigurationManager.GetSetting("FeatureToggle.RobotsAllowFeature") ?? "false"))
            {
                builder.AppendLine("Disallow: /");
            }

            return Content(builder.ToString(), "text/plain", Encoding.UTF8);
        }
    }
}
