using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("v1/apprenticeships")]
    public class SearchApprenticeshipVacanciesController : ApiController
    {
        private readonly SearchApprenticeshipVacanciesOrchestrator _searchOrchestrator;

        public SearchApprenticeshipVacanciesController(SearchApprenticeshipVacanciesOrchestrator searchOrchestrator)
        {
            _searchOrchestrator = searchOrchestrator;
        }

        /// <remarks>
        /// The apprenticeship search operation retrieves live apprenticeship vacancies based on search criteria specified 
        /// in the request parameters. 
        /// 
        /// Search criteria can be used to:
        /// 
        /// - Search by framework LARS code(s)
        /// - Search by standard LARS code(s)
        /// - Search by framework or standard LARS code(s)
        /// - Search by location (geopoint) and radius
        /// - Search for recently posted vacancies
        /// - Search for nationwide vacancies
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
        /// To search for nationwide vacancies:
        /// 
        /// ```
        /// /apprenticeships/search?nationwideOnly=true
        /// ```
        /// 
        /// #### Combining parameters ####
        /// 
        /// Multiple parameters can be added to the query string to refine the search. 
        /// Note that when specifying both framework and standard codes, the results will include vacancies with matching 
        /// framework or standard codes.
        /// 
        /// #### Sorting results ####
        /// 
        /// The results will be ordered by the following rules by default:
        /// - If searching by geo-location then the results are sorted by distance (closest first).
        /// - If searching by anything other than geo-location then the results are sorted by age (posted date) (newest first).
        /// 
        /// The default sorting rules can be overriden by using the `SortBy` query parameter. 
        /// SortBy can be set to "Age", "Distance" or "ExpectedStartDate".
        /// Whereas sorting by "Age" will return newest vacancies first, sorting by "ExpectedStartDate" will return vacancies that have earliest start date first.
        /// Beware that it is invalid to sort by distance if you have not searched by geo-location.
        /// 
        /// #### Error codes ####
        /// 
        /// The following error codes may be returned when calling this operation if any of the search criteria values 
        /// specified fail validation:
        /// 
        /// | Error code  | Explanation                                                                      |
        /// | ----------- | -------------------------------------------------------------------------------- |
        /// | 30100       | Search parameters not specified or insufficient                                  |
        /// | 30101       | Invalid Standard Code                                                            |
        /// | 30102       | Invalid Framework code                                                           |
        /// | 30103       | Invalid Page size                                                                |
        /// | 30104       | Invalid Page number                                                              |
        /// | 30105       | Invalid Posted in last number of days                                            |
        /// | 30106       | Invalid Latitude                                                                 |
        /// | 30107       | Invalid Longitude                                                                |
        /// | 30108       | Invalid Distance in miles                                                        |
        /// | 30109       | Invalid NationwideOnly                                                           |
        /// | 30110       | Invalid SortBy                                                                   |
        /// 
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        [Route("search")]
        [SwaggerOperation("SearchApprenticeshipVacancies", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(ApprenticeshipSearchResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        public async Task<IHttpActionResult> Search([FromUri(Name = "")]SearchApprenticeshipParameters searchApprenticeshipParameters)
        {
            var results =
                await _searchOrchestrator.SearchApprenticeship(searchApprenticeshipParameters)
                                         .ConfigureAwait(false);
            return Ok(results);
        }
    }
}
