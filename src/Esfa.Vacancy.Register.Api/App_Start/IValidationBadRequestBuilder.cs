using System.Net.Http;
using System.Web.Http;
using FluentValidation;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public interface IValidationBadRequestBuilder
    {
        IHttpActionResult CreateBadRequestResult(ValidationException validationException, HttpRequestMessage request);
    }
}