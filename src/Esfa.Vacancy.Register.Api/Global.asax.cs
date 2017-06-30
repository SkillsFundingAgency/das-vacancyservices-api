using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
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
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Info("Starting Web Role");

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            logger.Info("Web Role started");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            application?.Context?.Response.Headers.Remove("Server");

            _logger = DependencyResolver.Current.GetService<ILog>();

            var context = base.Context;
            _logger.Info($"{context.Request.HttpMethod} {context.Request.Url.PathAndQuery}");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            var logger = DependencyResolver.Current.GetService<ILog>();
            
            logger.Error(ex, "App_Error");
        }

    }
}