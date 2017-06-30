using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using FluentValidation;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public class CustomExceptionHandler : ExceptionHandler
    {
        private static readonly ILog Logger = DependencyResolver.Current.GetService<ILog>();

        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is ValidationException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var message = ((ValidationException) context.Exception).Message;
                response.Content = new StringContent(message);
                context.Result = new CustomErrorResult(context.Request, response);

                Logger.Warn(context.Exception, "Validation error");

                return;
            }

            Logger.Error(context.Exception, "Unhandled exception");

            base.Handle(context);
        }
    }
}
