using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.ExceptionHandling;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Infrastructure.Exceptions;
using FluentValidation;
using SFA.DAS.NLog.Logger;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public class VacancyApiExceptionHandler : ExceptionHandler
    {
        private const string GenericErrorMessage = "An internal error occurred, please try again.";
        private const string ExceptionInExceptionHandlerErrorMessage = "A critical error occurred, please try again.";

        public override void Handle(ExceptionHandlerContext context)
        {
            try
            {
                var logger = (ILog) GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILog));
                
                if (context.Exception is ValidationException)
                {
                    logger.Warn(context.Exception, GetLogMessage("Validation error", context));
                    context.Result = CreateBadRequestResult((ValidationException) context.Exception, context.Request);
                    return;
                }

                if (context.Exception is UnauthorisedException)
                {
                    logger.Warn(context.Exception, GetLogMessage("Authorisation error", context));
                    context.Result = CreateStringResult(HttpStatusCode.Unauthorized, ((UnauthorisedException) context.Exception).Message, context.Request);
                    return;
                }

                if (context.Exception is ResourceNotFoundException)
                {
                    logger.Warn(context.Exception, GetLogMessage("Unable to locate resource error", context));
                    context.Result = CreateStringResult(HttpStatusCode.NotFound, ((ResourceNotFoundException) context.Exception).Message, context.Request);
                    return;
                }

                if (context.Exception is InfrastructureException)
                {
                    logger.Error(context.Exception.InnerException, GetLogMessage("Unexpected infrastructure error", context));
                    context.Result = CreateStringResult(HttpStatusCode.InternalServerError, GenericErrorMessage, context.Request);
                    return;
                }

                logger.Error(context.Exception, GetLogMessage("Unexpected error", context));
                context.Result = CreateStringResult(HttpStatusCode.InternalServerError, GenericErrorMessage, context.Request);
            }
            catch
            {
                //whatever happens don't leak the stack trace
                context.Result = CreateStringResult(HttpStatusCode.InternalServerError, ExceptionInExceptionHandlerErrorMessage, context.Request);
            }
        }

        private IHttpActionResult CreateStringResult(HttpStatusCode statusCode, string content, HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(statusCode);
            response.Content = new StringContent(content);
            var result = new CustomErrorResult(request, response);
            return result;
        }

        private IHttpActionResult CreateBadRequestResult(ValidationException validationException, HttpRequestMessage request)
        {
            var badrequestResponse = new BadRequestResponse
            {
                RequestErrors = validationException.Errors
                    .Select(validationFailure => new BadRequestError
                    {
                        ErrorCode = validationFailure.ErrorCode,
                        ErrorMessage = validationFailure.ErrorMessage
                    })
            };

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new ObjectContent<BadRequestResponse>(badrequestResponse, new JsonMediaTypeFormatter())
            };

            return new CustomErrorResult(request, response);
        }

        private string GetLogMessage(string message, ExceptionHandlerContext context)
        {
            var requestUri = context.Request?.RequestUri?.ToString();
            
            return !string.IsNullOrEmpty(requestUri) ? $"{message} url:{requestUri}" : message;
        }
    }
}
