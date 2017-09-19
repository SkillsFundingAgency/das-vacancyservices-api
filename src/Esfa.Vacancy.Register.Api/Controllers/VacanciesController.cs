using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Register.Api.Attributes;
using Esfa.Vacancy.Register.Api.Orchestrators;
using SFA.DAS.NLog.Logger;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class VacanciesController : ApiController
    {
        private readonly ILog _log;
        private readonly IVacancyOrchestrator _vacancyOrchestrator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VacanciesController"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="vacancyOrchestrator">The vacancy orchestrator.</param>
        public VacanciesController(ILog log, IVacancyOrchestrator vacancyOrchestrator)
        {
            _log = log;
            _vacancyOrchestrator = vacancyOrchestrator;
        }

        /// <summary>
        /// Check if a vacancy exists
        /// </summary>
        /// <param name="vacancyReference">The vacancy reference.</param>
        /// <returns></returns>
        [SwaggerOperation("Head")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ExceptionHandling]
        [Route("api/vacancies/{vacancyReference}")]
        public async Task Head(string vacancyReference)
        {
            await Get(vacancyReference);
        }

        /// <summary>
        /// Get a vacancy by the public vacancy reference identifier
        /// </summary>
        /// <param name="vacancyReference">The vacancy reference number.</param>
        /// <returns>
        /// A vacancy for an apprenticeship or a traineeship
        /// </returns>
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.Vacancy))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ExceptionHandling]
        [Route("api/vacancies/{vacancyReference}")]
        public async Task<IHttpActionResult> Get(string vacancyReference)
        {
            var vacancy = await _vacancyOrchestrator.GetVacancyDetailsAsync(int.Parse(vacancyReference));

            return Ok(vacancy);
        }
    }
}
