using System.Web.Mvc;
using System.Web.Routing;

namespace Esfa.Vacancy.Register.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GetRobotsText",
                url: "robots.txt",
                defaults: new { controller = "Home", action = "RobotsText", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "ApiPage",
                url: "api",
                defaults: new { controller = "Home", action = "Api" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
