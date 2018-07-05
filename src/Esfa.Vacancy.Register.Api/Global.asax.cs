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
            logger.Info("Vacancy Register API service started");
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current?.Response.Headers.Remove("Server");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var ex = Server.GetLastError();

                var logger = DependencyResolver.Current.GetService<ILog>();
                if (ex != null) logger.Warn(ex, ex.Message);

                if (ex is HttpException)
                {
                    ApiRedirect("~/error");
                }
            }
            catch
            {
                // ignored
            }
        }

        private void ApiRedirect(string path)
        {
            Response.Clear();
            Server.ClearError();
            Context.RewritePath(path, false);
        }
    }
}
