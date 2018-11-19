using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("v1/traineeships")]
    public class GetTraineeshipVacancyController : ApiController
    {
        private readonly GetTraineeshipVacancyOrchestrator _vacancyOrchestrator;

        public GetTraineeshipVacancyController(GetTraineeshipVacancyOrchestrator vacancyOrchestrator)
        {
            _vacancyOrchestrator = vacancyOrchestrator;
        }

        /// <summary>
        /// The traineeship operation retrieves a single live traineeship vacancy using the vacancy reference number.
        /// 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("{vacancyReference}")]
        [SwaggerOperation("GetTraineeshipVacancy", Tags = new[] { "Traineeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(TraineeshipVacancy))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Vacancy not found or vacancy status is not Live")]
        public async Task<IHttpActionResult> Get(string vacancyReference)
        {
            var vacancy = await _vacancyOrchestrator.GetTraineeshipVacancyDetailsAsync(vacancyReference);

            return Ok(vacancy);
        }
    }
}