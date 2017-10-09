using System;
using System.Globalization;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Esfa.Vacancy.Register.Api.App_Start;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Api
{
    public class Global : HttpApplication
    {
        private ILog _logger;

        public Global()
        {
            _logger = DependencyResolver.Current.GetService<ILog>();
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            MvcHandler.DisableMvcResponseHeader = true;

            _logger.Info("Starting Web Role");

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperConfig.Configure();

            _logger.Info("Web Role started");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            application?.Context?.Response.Headers.Remove("Server");
        }
    }
}
