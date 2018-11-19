using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Constants;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("v1/apprenticeships")]
    public class GetApprenticeshipVacancyController : ApiController
    {
        private readonly GetApprenticeshipVacancyOrchestrator _apprenticeshipVacancyOrchestrator;

        public GetApprenticeshipVacancyController(GetApprenticeshipVacancyOrchestrator apprenticeshipVacancyOrchestrator)
        {
            _apprenticeshipVacancyOrchestrator = apprenticeshipVacancyOrchestrator;
        }

        /// <summary>
        /// The apprenticeship operation retrieves a single live apprenticeship vacancy using the vacancy reference number.
        /// 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("{vacancyReference:regex(^(?i)(?!search).*$)}", Name = RouteName.GetApprenticeshipVacancyByReference)]
        [SwaggerOperation("GetApprenticeshipVacancy", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(ApprenticeshipVacancy))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Vacancy not found or vacancy status is not Live")]
        public async Task<IHttpActionResult> Get(string vacancyReference)
        {
            var vacancy = await _apprenticeshipVacancyOrchestrator.GetApprenticeshipVacancyDetailsAsync(vacancyReference)
                                                                  .ConfigureAwait(false);
            return Ok(vacancy);
        }
    }
}
