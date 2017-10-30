using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Register.Api.Orchestrators;
using SFA.DAS.NLog.Logger;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/apprenticeships")]
    public class GetApprenticeshipVacancyController : ApiController
    {
        private readonly ILog _log;
        private readonly GetApprenticeshipVacancyOrchestrator _apprenticeshipVacancyOrchestrator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetApprenticeshipVacancyController"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="apprenticeshipVacancyOrchestrator">The vacancy orchestrator.</param>
        public GetApprenticeshipVacancyController(ILog log, GetApprenticeshipVacancyOrchestrator apprenticeshipVacancyOrchestrator)
        {
            _log = log;
            _apprenticeshipVacancyOrchestrator = apprenticeshipVacancyOrchestrator;
        }

        /// <summary>
        /// Get an apprenticeship vacancy by the public vacancy reference identifier
        /// </summary>
        /// <param name="vacancyReference">The vacancy reference number.</param>
        /// <returns>
        /// A vacancy for an apprenticeship
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("{vacancyReference:int}")]
        [SwaggerOperation("GetApprenticeshipVacancy", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.ApprenticeshipVacancy))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> Get(int vacancyReference)
        {
            var vacancy = await _apprenticeshipVacancyOrchestrator.GetApprenticeshipVacancyDetailsAsync(vacancyReference);

            return Ok(vacancy);
        }
    }
}
