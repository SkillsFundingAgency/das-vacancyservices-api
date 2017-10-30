using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("api/v1/apprenticeships")]
    public class SearchApprenticeshipVacanciesController : ApiController
    {
        private readonly SearchApprenticeshipVacanciesOrchestrator _searchOrchestrator;

        public SearchApprenticeshipVacanciesController(SearchApprenticeshipVacanciesOrchestrator searchOrchestrator)
        {
            _searchOrchestrator = searchOrchestrator;
        }

        /// <summary>
        /// The Vacancy Summary service provides system integrators with the ability to download open apprenticeships 
        /// from the Recruit an apprentice system. This service allows various parameters to allow the system 
        /// integrators to select the most relevant apprenticeships and/or request up-to date details after a given date.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("search")]
        [SwaggerOperation("SearchApprenticeshipVacancies", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(ApprenticeshipSummary))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestError))]
        public async Task<IHttpActionResult> Search([FromUri]SearchApprenticeshipParameters searchApprenticeshipParameters)
        {
            var results = await _searchOrchestrator.SearchApprenticeship(searchApprenticeshipParameters);
            return Ok(results);
        }
    }
}
