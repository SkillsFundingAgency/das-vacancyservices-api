﻿using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("api/v1/traineeships")]
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
        /// /traineeships/1234567
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
        [SwaggerOperation("GetTraineeshipVacancy", Tags = new[] { "Traineeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.TraineeshipVacancy))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestError))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Vacancy not found")]
        public async Task<IHttpActionResult> Get(int vacancyReference)
        {
            var vacancy = await _vacancyOrchestrator.GetTraineeshipVacancyDetailsAsync(vacancyReference);

            return Ok(vacancy);
        }
    }
}