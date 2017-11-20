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
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            MvcHandler.DisableMvcResponseHeader = true;

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var logger = DependencyResolver.Current.GetService<ILog>();
            logger.Info("Web Role started");
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current?.Response.Headers.Remove("Server");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();

            if (ex is HttpException httpError)
            {
                ApiRedirect("~/api/error");
                return;
            }

            var logger = DependencyResolver.Current.GetService<ILog>();
            if (ex != null) logger.Warn(ex, ex.Message);
        }

        private void ApiRedirect(string path)
        {
            Response.Clear();
            Server.ClearError();
            Context.RewritePath(path, false);
        }
    }
}
