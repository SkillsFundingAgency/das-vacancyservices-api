using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Manage.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var logger = DependencyResolver.Current.GetService<ILog>();
            logger.Info("Vacancy Manage API service started");
        }
    }
}
