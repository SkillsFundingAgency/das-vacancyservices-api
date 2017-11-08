using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using SFA.DAS.NLog.Logger;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("api/v1/apprenticeships")]
    public class GetApprenticeshipVacancyController : ApiController
    {
        private readonly ILog _log;
        private readonly GetApprenticeshipVacancyOrchestrator _apprenticeshipVacancyOrchestrator;

        public GetApprenticeshipVacancyController(ILog log, GetApprenticeshipVacancyOrchestrator apprenticeshipVacancyOrchestrator)
        {
            _log = log;
            _apprenticeshipVacancyOrchestrator = apprenticeshipVacancyOrchestrator;
        }

        /// <summary>
        /// The apprenticeship operation retrieves a single live apprenticeship vacancy using the vacancy reference number.
        /// 
        /// Note that:
        /// 
        /// - the vacancy reference number should be specified as a number (ie. excluding any prefix)
        /// - only live vacancies can be retrieved using this operation
        /// 
        /// #### Example ####
        /// 
        /// To retrieve VAC001234567:
        /// 
        /// ```
        /// /apprenticeships/1234567
        /// ```
        /// 
        /// #### Error codes ####
        /// 
        /// The following error codes may be returned when calling this operation:
        /// 
        /// | Error code  | Explanation                                                    |
        /// | ----------- | -------------------------------------------------------------- |
        /// | 30201       | Vacancy reference number must be greater than 0                |
        /// 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("{vacancyReference:int}")]
        [SwaggerOperation("GetApprenticeshipVacancy", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.ApprenticeshipVacancy))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestError))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Vacancy not found or vacancy status is not Live")]
        public async Task<IHttpActionResult> Get(int vacancyReference)
        {
            var vacancy = await _apprenticeshipVacancyOrchestrator.GetApprenticeshipVacancyDetailsAsync(vacancyReference);

            return Ok(vacancy);
        }
    }
}
