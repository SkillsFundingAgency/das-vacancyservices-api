using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.App_Start;
using FluentValidation;

namespace Esfa.Vacancy.Register.Api.Validation
{
    public class ValidationBadRequestBuilder : IValidationBadRequestBuilder
    {
        public IHttpActionResult CreateBadRequestResult(ValidationException validationException, HttpRequestMessage request)
        {
            var badRequestContent = new BadRequestContent
            {
                RequestErrors = validationException.Errors
                    .Select(validationFailure => new BadRequestError
                    {
                        ErrorCode = validationFailure.ErrorCode,
                        ErrorMessage = validationFailure.ErrorMessage
                    })
            };

            var response = request.CreateResponse(HttpStatusCode.BadRequest, badRequestContent);

            return new CustomErrorResult(request, response);
        }
    }
}