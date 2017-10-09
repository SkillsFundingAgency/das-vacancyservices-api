﻿using System.Net;
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
    public class GetVacancyController : ApiController
    {
        private readonly ILog _log;
        private readonly GetVacancyOrchestrator _vacancyOrchestrator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetVacancyController"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="vacancyOrchestrator">The vacancy orchestrator.</param>
        public GetVacancyController(ILog log, GetVacancyOrchestrator vacancyOrchestrator)
        {
            _log = log;
            _vacancyOrchestrator = vacancyOrchestrator;
        }

        /// <summary>
        /// Get a vacancy by the public vacancy reference identifier
        /// </summary>
        /// <param name="vacancyReference">The vacancy reference number.</param>
        /// <returns>
        /// A vacancy for an apprenticeship or a traineeship
        /// </returns>
        [AllowAnonymous]
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.Vacancy))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ExceptionHandling]
        [Route("api/v1/vacancies/{vacancyReference}")]
        public async Task<IHttpActionResult> Get(int vacancyReference)
        {
            var vacancy = await _vacancyOrchestrator.GetVacancyDetailsAsync(vacancyReference);

            return Ok(vacancy);
        }
    }
}