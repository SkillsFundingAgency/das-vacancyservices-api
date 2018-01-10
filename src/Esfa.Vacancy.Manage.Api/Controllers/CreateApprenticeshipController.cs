using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Manage.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Manage.Api.Controllers
{
    public class CreateApprenticeshipController : ApiController
    {
        private readonly CreateApprenticeshipOrchestrator _orchestrator;

        public CreateApprenticeshipController(CreateApprenticeshipOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }
        /// <summary>
        /// Creates the specified apprenticeship vacancy.
        /// 
        /// #### Error codes ####
        /// 
        /// The following error codes may be returned when calling this operation if any of the vacancy values 
        /// specified fail validation:
        /// 
        /// | Error code  | Explanation                                                                      |
        /// | ----------- | -------------------------------------------------------------------------------- |
        /// | 31000       | Title can't be empty                                                        |
        /// | 31001       | Title can't be longer than 100 characters                                        |
        /// | 31002       | Title must contain the word 'apprentice' or 'apprenticeship'                     |
        /// | 31003       | Title can't contain invalid characters                                           |
        /// | 31004       | Short description can't be empty                                            |
        /// | 31005       | Short description can't be longer than 350 characters                            |
        /// | 31006       | Short description can't contain invalid characters                               |
        /// | 31007       | Long description can't be empty                                             |
        /// | 31008       | Long description can't contain invalid characters                                |
        /// | 31009       | Long description can't contain blacklisted HTML elements                         |
        /// | 31010       | Application closing date is required                                             |
        /// | 31011       | Application closing date be after tomorrow                                       |
        /// | 31012       | Expected start date is required                                                  |
        /// | 31013       | Expected start date must be after application closing date                       |
        /// 
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/apprenticeships")]
        [SwaggerOperation("CreateApprenticeshipVacancy", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(CreateApprenticeshipResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        public async Task<IHttpActionResult> Create([FromBody]CreateApprenticeshipParameters createApprenticeshipParameters)
        {
            var result = await _orchestrator.CreateApprecticeshipAsync(createApprenticeshipParameters);
            return Ok(result);
        }
    }
}
