using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/traineeships")]
    public class GetTraineeshipVacancyController : ApiController
    {
        private readonly GetTraineeshipVacancyOrchestrator _vacancyOrchestrator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTraineeshipVacancyController"/> class.
        /// </summary>
        /// <param name="vacancyOrchestrator">The traineeship vacancy orchestrator.</param>
        public GetTraineeshipVacancyController(GetTraineeshipVacancyOrchestrator vacancyOrchestrator)
        {
            _vacancyOrchestrator = vacancyOrchestrator;
        }

        /// <summary>
        /// Get a traineeship vacancy by the public vacancy reference identifier
        /// </summary>
        /// <param name="vacancyReference">The vacancy reference number.</param>
        /// <returns>
        /// A vacancy for a traineeship
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("{vacancyReference}")]
        [SwaggerOperation("GetTraineeshipVacancy", Tags = new[] { "Traineeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.TraineeshipVacancy))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public async Task<IHttpActionResult> Get(int vacancyReference)
        {
            var vacancy = await _vacancyOrchestrator.GetTraineeshipVacancyDetailsAsync(vacancyReference);

            return Ok(vacancy);
        }
    }
}