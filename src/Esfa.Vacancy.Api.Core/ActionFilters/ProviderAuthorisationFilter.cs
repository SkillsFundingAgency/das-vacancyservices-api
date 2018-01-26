using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Esfa.Vacancy.Api.Core.Extensions;
using Esfa.Vacancy.Application.Exceptions;

namespace Esfa.Vacancy.Api.Core.ActionFilters
{
    public class ProviderAuthorisationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var headerValue = actionContext.Request.GetHeaderValue(Constants.RequestHeaderNames.UserNote);

            int ukprn;
            var result = TryExtractUkprnFromHeader(headerValue, out ukprn);
            if (!result)
            {
                throw new UnauthorisedException(Constants.AuthorisationErrorMessages.InvalidUkprn);
            }

            actionContext.Request.Headers.Add(Constants.RequestHeaderNames.ProviderUkprn, ukprn.ToString());

            base.OnActionExecuting(actionContext);
        }

        private static bool TryExtractUkprnFromHeader(string userNoteHeader, out int providerUkprn)
        {
            providerUkprn = 0;

            if (userNoteHeader == null) return false;

            var strippedValue = userNoteHeader?
                .ToLower()
                .Replace(" ", string.Empty)
                .Replace("ukprn=", string.Empty);

            return int.TryParse(strippedValue, out providerUkprn);
        }
    }
}