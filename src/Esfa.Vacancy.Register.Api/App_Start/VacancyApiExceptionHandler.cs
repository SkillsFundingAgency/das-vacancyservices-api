using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Infrastructure.Exceptions;
using FluentValidation;
using SFA.DAS.NLog.Logger;
using System.Web.Http;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public class VacancyApiExceptionHandler : ExceptionHandler
    {
        private readonly IValidationBadRequestBuilder _validationBadRequestBuilder;

        public VacancyApiExceptionHandler(IValidationBadRequestBuilder validationBadRequestBuilder)
        {
            _validationBadRequestBuilder = validationBadRequestBuilder;
        }

        private const string GenericErrorMessage = "An internal error occurred, please try again.";
        private const string ExceptionInExceptionHandlerErrorMessage = "A critical error occurred, please try again.";

        public override void Handle(ExceptionHandlerContext context)
        {
            try
            {
                var logger = (ILog) GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILog));
                
                if (context.Exception is ValidationException)
                {
                    logger.Warn(context.Exception, FormatLogMessage("Validation error", context));
                    context.Result = _validationBadRequestBuilder.CreateBadRequestResult((ValidationException) context.Exception, context.Request);
                    return;
                }

                if (context.Exception is UnauthorisedException)
                {
                    logger.Warn(context.Exception, FormatLogMessage("Authorisation error", context));
                    context.Result = CreateStringResult(HttpStatusCode.Unauthorized, ((UnauthorisedException) context.Exception).Message, context.Request);
                    return;
                }

                if (context.Exception is ResourceNotFoundException)
                {
                    logger.Warn(context.Exception, FormatLogMessage("Unable to locate resource error", context));
                    context.Result = CreateStringResult(HttpStatusCode.NotFound, ((ResourceNotFoundException) context.Exception).Message, context.Request);
                    return;
                }

                if (context.Exception is InfrastructureException)
                {
                    logger.Error(context.Exception.InnerException, FormatLogMessage("Unexpected infrastructure error", context));
                    context.Result = CreateStringResult(HttpStatusCode.InternalServerError, GenericErrorMessage, context.Request);
                    return;
                }

                logger.Error(context.Exception, FormatLogMessage("Unexpected error", context));
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

        private string FormatLogMessage(string message, ExceptionHandlerContext context)
        {
            var requestUri = context.Request?.RequestUri?.ToString();
            
            return !string.IsNullOrEmpty(requestUri) ? $"{message} url:{requestUri}" : message;
        }
    }
}
