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
            var req = Request;
            var res = Response;

            if (!(ex is HttpException httpError)) return;

            if (Request.Path.StartsWith("api/"))
            {
                Response.Redirect("api/DangerousRequest");
            }
            else
            {
                Response.Redirect("/DangerousRequest");
            }

            var logger = DependencyResolver.Current.GetService<ILog>();
            logger.Error(httpError, httpError.Message);
        }
    }
}
