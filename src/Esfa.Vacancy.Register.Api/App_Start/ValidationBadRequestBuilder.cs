using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using FluentValidation;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public class ValidationBadRequestBuilder : IValidationBadRequestBuilder
    {
        public IHttpActionResult CreateBadRequestResult(ValidationException validationException, HttpRequestMessage request)
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

            var response = request.CreateResponse(HttpStatusCode.BadRequest, badrequestResponse);

            return new CustomErrorResult(request, response);
        }
    }
}