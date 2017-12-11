using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Manage.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Manage.Api.Controllers
{
    public class CreateApprenticeshipController : ApiController
    {
        private readonly CreateApprenticeshipOrchestrator _orchestrator;

        public CreateApprenticeshipController(CreateApprenticeshipOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }
        /// <summary>
        /// Creates the specified apprenticeship vacancy.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/apprenticeships")]
        [SwaggerOperation("CreateApprenticeshipVacancy", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(CreateApprenticeshipResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        public async Task<IHttpActionResult> Create([FromBody]CreateApprenticeshipParameters createApprenticeshipParameters)
        {
            var result = await _orchestrator.CreateApprecticeshipAsync(createApprenticeshipParameters);
            return Ok(result);
        }
    }
}
