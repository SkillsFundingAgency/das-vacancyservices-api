using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    public class GlobalErrorController : ApiController
    {
        private readonly ILog _logger;

        public GlobalErrorController(ILog logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("api/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage Get()
        {
            try
            {
                _logger.Warn("There was a problem with the request. Returning HTTP 400.");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "There was a problem with your request.");
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}
