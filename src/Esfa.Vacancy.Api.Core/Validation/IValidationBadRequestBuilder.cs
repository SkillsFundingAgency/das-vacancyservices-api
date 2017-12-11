using System.Net.Http;
using System.Web.Http;
using FluentValidation;

namespace Esfa.Vacancy.Api.Core.Validation
{
    public interface IValidationBadRequestBuilder
    {
        IHttpActionResult CreateBadRequestResult(ValidationException validationException, HttpRequestMessage request);
    }
}