using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("api/v1/apprenticeships")]
    public class SearchApprenticeshipVacanciesController : ApiController
    {
        private readonly SearchApprenticeshipVacanciesOrchestrator _searchOrchestrator;

        public SearchApprenticeshipVacanciesController(SearchApprenticeshipVacanciesOrchestrator searchOrchestrator)
        {
            _searchOrchestrator = searchOrchestrator;
        }

        /// <summary>
        /// The apprenticeship search operation retrieves live apprenticeship vacancies based on search criteria specified 
        /// in the request parameters. 
        /// 
        /// Search criteria can be used to:
        /// 
        /// - Search by framework LARS code(s)
        /// - Search by standard LARS code(s)
        /// - Search by framework or standard LARS code(s)
        /// - Search for recently posted vacancies
        /// 
        /// #### Data paging ####
        /// 
        /// Search results are returned in pages of data. 
        /// If not specified then the default page size is 100 vacancies. 
        /// If the search yields more data than can be included in a single page then additional pages can be requested by 
        /// specifying a specific page number in the request. eg. pageNumber=2
        /// 
        /// #### Examples ####
        /// 
        /// To search for vacancies with standard code 94:
        /// 
        /// ```
        /// /apprenticeships/search?standardLarsCodes=94
        /// ```
        /// 
        /// Multiple standard codes can be specified by using a comma delimited list of standard codes. 
        /// To search for vacancies with either standard code 94 or 95:
        /// 
        /// ```
        /// /apprenticeships/search?standardLarsCodes=94,95
        /// ```
        /// 
        /// To search for vacancies that went live within the last 2 days:
        /// 
        /// ```
        /// /apprenticeships/search?postedInLastNumberOfDays=2
        /// ```
        /// 
        /// To search for vacancies that went live today (0 days ago):
        /// 
        /// ```
        /// /apprenticeships/search?postedInLastNumberOfDays=0
        /// ```
        /// 
        /// #### Combining parameters ####
        /// 
        /// Multiple parameters can be added to the query string to refine the search. 
        /// Note that when specifying both framework and standard codes, the results will include vacancies with matching 
        /// framework or standard codes.
        /// 
        /// #### Error codes ####
        /// 
        /// The following error codes may be returned when calling this operation if any of the search criteria values 
        /// specified fail validation:
        /// 
        /// | Error code  | Explanation                                                    |
        /// | ----------- | -------------------------------------------------------------- |
        /// | 30100       | Search parameters were not specified                           |
        /// | 30101       | At least 1 standard *or* framework code must be specified      |
        /// | 30102       | Standard code must be a number                                 |
        /// | 30103       | Framework code must be a number                                |
        /// | 30104       | Page size must be between 1 and 250 (inclusive)                |
        /// | 30105       | Page number must be greater than 0                             |
        /// | 30106       | Number of days since posted must be greater than or equal to 0 |
        /// | 30107       | Framework code not recognised                                  |
        /// | 30108       | Standard code not recognised                                   |
        /// | 30109       | Latitude is required when performing geo-search                |
        /// | 30110       | Latitude must be between -90 and 90 (inclusive)                |
        /// | 30111       | Longitude is required when performing geo-search               |
        /// | 30112       | Longitude must be between -180 and 180 (inclusive)             |
        /// | 30113       | Distance in miles is required when performing geo-search       |
        /// | 30114       | Distance in miles must be between 1 and 1000 (inclusive)       |
        /// 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("search")]
        [SwaggerOperation("SearchApprenticeshipVacancies", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(ApprenticeshipSummary))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        public async Task<IHttpActionResult> Search([FromUri(Name = "")]SearchApprenticeshipParameters searchApprenticeshipParameters)
        {
            var results = await _searchOrchestrator.SearchApprenticeship(searchApprenticeshipParameters);
            return Ok(results);
        }
    }
}
