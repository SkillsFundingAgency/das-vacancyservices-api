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
        /// | Error code  | Explanation                                 |
        /// | ----------- | ------------------------------------------- |
        /// | 31000       | Invalid Request body                        |
        /// | 31001       | Invalid Title                               |
        /// | 31002       | Invalid Short description                   |
        /// | 31003       | Invalid Long description                    |
        /// | 31004       | Invalid Application closing date            |
        /// | 31005       | Invalid Expected start date                 |
        /// | 31006       | Invalid Working week                        |
        /// | 31007       | Invalid Hours per week                      |
        /// | 31008       | Invalid Wage type                           |
        /// | 31009       | Invalid Min wage                            |
        /// | 31010       | Invalid Max wage                            |
        /// | 31011       | Invalid Location type                       |
        /// | 31012       | Invalid Address line 1                      |
        /// | 31013       | Invalid Address line 2                      |
        /// | 31014       | Invalid Address line 3                      |
        /// | 31015       | Invalid Address line 4                      |
        /// | 31016       | Invalid Address line 5                      |
        /// | 31017       | Invalid Town                                |
        /// | 31018       | Invalid Postcode                            |
        /// | 31019       | Invalid Number of positions                 |
        /// | 31020       | Invalid Provider's Ukprn                    |
        /// | 31021       | Invalid Employer's Edsurn                   |
        /// | 31022       | Invalid Provider site's Edsurn              |
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
