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

        public VacanciesController(ILog log, IVacancyOrchestrator vacancyOrchestrator)
        {
            _log = log;
            _vacancyOrchestrator = vacancyOrchestrator;
        }

        /// <summary>
        /// Check if a vacancy exists
        /// </summary>
        /// <param name="id"></param>
        [SwaggerOperation("Head")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ExceptionHandling]
        public async Task Head(int id)
        {
            await Get(id);
        }

        /// <summary>
        /// Get a vacancy by the public vacancy reference identifier
        /// </summary>
        /// <param name="id">The public vacancy reference identifier</param>
        /// <returns>A vacancy for an apprenticeship or a traineeship</returns>
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.Vacancy))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ExceptionHandling]
        [Route("api/vacancies/{vacancyReference}")]
        public async Task<IHttpActionResult> Get(int vacancyReference)
        {
            var vacancy = await _vacancyOrchestrator.GetVacancyDetailsAsync(vacancyReference);

            return Ok(vacancy);
        }
    }
}
