using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    public class GlobalErrorController : ApiController
    {
        [AllowAnonymous]
        [Route("api/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There was a problem with your request.");
            }
            catch {}

            return null;
        }
    }
}
