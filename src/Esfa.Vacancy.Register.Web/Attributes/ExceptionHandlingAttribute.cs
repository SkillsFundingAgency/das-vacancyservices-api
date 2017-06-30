using System;
using System.Web.Http.Filters;
using System.Web.Mvc;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Web.Attributes
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Error(context.Exception, $"App_Error {context.Request?.RequestUri}");
        }
    }
}
