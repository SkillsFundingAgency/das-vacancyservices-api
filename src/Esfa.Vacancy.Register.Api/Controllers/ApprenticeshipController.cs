using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Attributes;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [ExceptionHandling]
    public class ApprenticeshipController : ApiController
    {
        private readonly ISearchOrchestrator _searchOrchestrator;

        public ApprenticeshipController(ISearchOrchestrator searchOrchestrator)
        {
            _searchOrchestrator = searchOrchestrator;
        }

        /// <summary>
        /// The Vacancy Summary service provides system integrators with the ability to download open apprenticeships 
        /// from the Recruit an apprentice system. This service allows various parameters to allow the system 
        /// integrators to select the most relevant apprenticeships and/or request up-to date details after a given date.
        /// </summary>
        /// <param name="searchApprenticeshipParameters">The search apprenticeship parameters.</param>
        /// <returns></returns>
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(ApprenticeshipSummary))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("api/v1/apprenticeship/search")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Search([FromUri]SearchApprenticeshipParameters searchApprenticeshipParameters)
        {
            var results = await _searchOrchestrator.SearchApprenticeship(searchApprenticeshipParameters);
            return Ok(results);
        }
    }
}
