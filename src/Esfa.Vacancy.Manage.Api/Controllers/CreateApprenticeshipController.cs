using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Core.ActionFilters;
using Esfa.Vacancy.Api.Core.Extensions;
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
        /// | 31000       | Title can't be empty                                                             |
        /// | 31001       | Title can't be longer than 100 characters                                        |
        /// | 31002       | Title must contain the word 'apprentice' or 'apprenticeship'                     |
        /// | 31003       | Title can't contain invalid characters                                           |
        /// | 31004       | Short description can't be empty                                                 |
        /// | 31005       | Short description can't be longer than 350 characters                            |
        /// | 31006       | Short description can't contain invalid characters                               |
        /// | 31007       | Long description can't be empty                                                  |
        /// | 31008       | Long description can't contain invalid characters                                |
        /// | 31009       | Long description can't contain blacklisted HTML elements                         |
        /// | 31010       | Application closing date can't be empty                                          |
        /// | 31011       | Application closing date be after tomorrow                                       |
        /// | 31012       | Expected start date can't be empty                                               |
        /// | 31013       | Expected start date must be after application closing date                       |
        /// | 31014       | The request must be valid json/xml and contain required values                   |
        /// | 31015       | Working week can't be empty                                                      |
        /// | 31016       | Working week can't be longer than 250 characters                                 |
        /// | 31017       | Working week can't contain invalid characters                                    |
        /// | 31018       | Hours per week can't be empty                                                    |
        /// | 31019       | Hours per week must be between 16 and 48                                         |
        /// | 31031       | Location type can't be empty                                                     |
        /// | 31032       | Address line 1 can't be empty when location type is other location               |
        /// | 31033       | Address line 1 can't be longer than 300 characters                               |
        /// | 31034       | Address line 1 can't contain invalid characters                                  |
        /// | 31035       | Address line 2 can't be empty when location type is other location               |
        /// | 31036       | Address line 2 can't be longer than 300 characters                               |
        /// | 31037       | Address line 2 can't contain invalid characters                                  |
        /// | 31038       | Address line 3 can't be empty when location type is other location               |
        /// | 31039       | Address line 3 can't be longer than 300 characters                               |
        /// | 31040       | Address line 3 can't contain invalid characters                                  |
        /// | 31041       | Address line 4 can't be longer than 300 characters                               |
        /// | 31042       | Address line 4 can't contain invalid characters                                  |
        /// | 31043       | Address line 5 can't be longer than 300 characters                               |
        /// | 31044       | Address line 5 can't contain invalid characters                                  |
        /// | 31045       | Town can't be empty when location type is other location                         |
        /// | 31046       | Town can't be longer than 100 characters                                         |
        /// | 31047       | Town can't contain invalid characters                                            |
        /// | 31048       | Postcode can't be empty when location type is other location                     |
        /// | 31049       | Postcode must be a valid UK postcode                                             |
        /// | 31201       | Number of positions can't be empty and has to less than equal to 5000            |
        /// 
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProviderAuthorisationFilter]
        [Route("api/v1/apprenticeships")]
        [SwaggerOperation("CreateApprenticeshipVacancy", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(CreateApprenticeshipResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Invalid provider ukprn", typeof(StringContent))]
        public async Task<IHttpActionResult> Create([FromBody]CreateApprenticeshipParameters createApprenticeshipParameters)
        {
            var headers = Request.GetApimUserContextHeaders();
            var result = await _orchestrator.CreateApprenticeshipAsync(createApprenticeshipParameters, headers);
            return Ok(result);
        }
    }
}
