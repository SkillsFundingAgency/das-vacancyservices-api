using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Infrastructure.Exceptions;
using FluentValidation;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Api.Core
{
    public class VacancyApiExceptionHandler : ExceptionHandler
    {
        private readonly IValidationBadRequestBuilder _validationBadRequestBuilder;
        private readonly ILog _logger;

        public VacancyApiExceptionHandler(IValidationBadRequestBuilder validationBadRequestBuilder, ILog logger)
        {
            _validationBadRequestBuilder = validationBadRequestBuilder;
            _logger = logger;
        }

        private const string GenericErrorMessage = "An internal error occurred, please try again.";
        private const string ExceptionInExceptionHandlerErrorMessage = "A critical error occurred, please try again.";

        public override void Handle(ExceptionHandlerContext context)
        {
            try
            {
                if (context.Exception is ValidationException)
                {
                    _logger.Info(FormatLogMessage("Validation error", context));
                    context.Result = _validationBadRequestBuilder.CreateBadRequestResult((ValidationException)context.Exception, context.Request);
                    return;
                }

                if (context.Exception is UnauthorisedException)
                {
                    _logger.Warn(context.Exception, FormatLogMessage("Authorisation error", context));
                    context.Result = CreateResult(HttpStatusCode.Unauthorized, ((UnauthorisedException)context.Exception).Message, context.Request);
                    return;
                }

                if (context.Exception is ResourceNotFoundException)
                {
                    _logger.Info(FormatLogMessage("Unable to locate resource error", context));
                    context.Result = CreateResult(HttpStatusCode.NotFound, ((ResourceNotFoundException)context.Exception).Message, context.Request);
                    return;
                }

                if (context.Exception is InfrastructureException)
                {
                    _logger.Error(context.Exception.InnerException, FormatLogMessage("Unexpected infrastructure error", context));
                    context.Result = CreateResult(HttpStatusCode.InternalServerError, GenericErrorMessage, context.Request);
                    return;
                }

                _logger.Error(context.Exception, FormatLogMessage("Unexpected error", context));
                context.Result = CreateResult(HttpStatusCode.InternalServerError, GenericErrorMessage, context.Request);
            }
            catch
            {
                //whatever happens don't leak the stack trace
                context.Result = CreateResult(HttpStatusCode.InternalServerError, ExceptionInExceptionHandlerErrorMessage, context.Request);
            }
        }

        private string FormatLogMessage(string message, ExceptionHandlerContext context)
        {
            var requestUri = context.Request?.RequestUri?.ToString();

            return !string.IsNullOrEmpty(requestUri) ? $"{message} url:{requestUri}" : message;
        }

        private IHttpActionResult CreateResult(HttpStatusCode statusCode, string content, HttpRequestMessage request)
        {
            var response = request.CreateErrorResponse(statusCode, content);
            return new CustomErrorResult(request, response);
        }
    }
}
