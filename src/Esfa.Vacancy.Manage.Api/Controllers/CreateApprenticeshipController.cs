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
        /// The apprenticeship operation creates an apprenticeship vacancy using the specified values.
        /// 
        /// #### Restricted values ####
        /// 
        /// These fields will only accept specific values as listed below:
        /// * ApplicationMethod
        ///     - Online
        ///     - Offline
        /// * LocationType
        ///     - OtherLocation
        ///     - EmployerLocation
        ///     - Nationwide
        /// * DurationType
        ///     - Weeks
        ///     - Months
        ///     - Years
        /// * WageType
        ///     - CustomWageFixed
        ///     - CustomWageRange
        ///     - NationalMinimumWage
        ///     - ApprenticeshipMinimumWage
        ///     - Unwaged
        ///     - CompetitiveSalary
        ///     - ToBeSpecified
        /// * WageUnit
        ///     - NotApplicable
        ///     - Weekly
        ///     - Monthly
        ///     - Annually
        /// * TrainingType
        ///     - Framework
        ///     - Standard
        /// 
        /// #### Validation rules ####
        /// 
        /// When creating a vacancy the following rules must be considered. 
        /// 1. All values are required to be populated except in the following cases:
        /// 
        /// * For all vacancies these values are optional
        ///     - ContactName
        ///     - ContactEmail
        ///     - ContactNumber
        ///     - ThingsToConsider
        /// 
        /// * For an Online vacancy
        ///     - SupplementaryQuestion1 and SupplementaryQuestion2 are optional
        ///     - ExternalApplicationUrl and ExternalApplicationInstructions must be empty
        /// 
        /// * For an Offline vacancy
        ///     - ExternalApplicationInstructions is optional
        ///     - SupplementaryQuestion1 and SupplementaryQuestion2 must be empty
        /// 
        /// * When LocationType is EmployerLocation or Nationwide 
        ///     - All address fields must be empty.
        /// 
        /// * When LocationType is OtherLocation 
        ///     - Only Address1, Town and Postcode are required
        ///     - All other address fields are optional
        /// 
        /// * When WageType is CustomWageFixed
        ///     - WageTypeReason must be empty
        ///     - WageUnit must be a value other than NotApplicable
        ///     - MinWage and MaxWage must be empty
        ///     - FixedWage must be greater than or equal to the Apprenticeship minimum wage 
        /// 
        /// * When WageType is CustomWageRange
        ///     - WageTypeReason must be empty
        ///     - WageUnit must be a value other than NotApplicable
        ///     - MinWage must be greater than or equal to the Apprenticeship minimum wage 
        ///     - MaxWage must be greater than MinWage
        ///     - FixedWage must be empty
        /// 
        /// * When WageType is NationalMinimumWage or ApprenticeshipMinimumWage
        ///     - MinWage, MaxWage, FixedWage and WageTypeReason must be empty
        ///     - WageUnit must be NotApplicable
        /// 
        /// * When WageType is Unwaged, CompetitiveSalary or ToBeSpecified
        ///     - FixedWage, MinWage and MaxWage must be empty
        ///     - WageUnit must be NotApplicable
        /// 
        /// * When TrainingType is Framework
        ///     - TrainingCode should be in format ###-##-##
        /// 
        /// * When TrainingType is Standard
        ///     - TrainingCode should be a numeric value no greater than 9999
        /// 
        /// * Text fields will take a maximum of 4,000 characters except for the following
        ///     - Title = 100
        ///     - ShortDescription = 350
        ///     - ContactName = 100
        ///     - ContactEmail = 100
        ///     - AddressLine = 300
        ///     - Town = 100
        ///     - WageTypeReason = 240
        ///     - WorkingWeek = 250
        /// 
        /// * Additional rules
        ///     - Title must include the word ***apprentice***
        ///     - HoursPerWeek must be between 16 and 48 inclusive
        ///     - ExpectedDuration must be a minimum of 1 year, 12 months or 52 weeks depending on the value of DurationType selected
        ///     - NumberOfPositions must not exceed 5,000
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
        /// | 31009       | Invalid Wage type reason                    |
        /// | 31010       | Invalid Wage unit                           |
        /// | 31011       | Invalid Fixed wage                          |
        /// | 31012       | Invalid Min wage                            |
        /// | 31013       | Invalid Max wage                            |
        /// | 31014       | Invalid Expected duration                   |
        /// | 31015       | Invalid Duration type                       |
        /// | 31016       | Invalid Location type                       |
        /// | 31017       | Invalid Address line 1                      |
        /// | 31018       | Invalid Address line 2                      |
        /// | 31019       | Invalid Address line 3                      |
        /// | 31020       | Invalid Address line 4                      |
        /// | 31021       | Invalid Address line 5                      |
        /// | 31022       | Invalid Town                                |
        /// | 31023       | Invalid Postcode                            |
        /// | 31024       | Invalid Number of positions                 |
        /// | 31025       | Invalid Provider's Ukprn                    |
        /// | 31026       | Invalid Employer's Edsurn                   |
        /// | 31027       | Invalid Provider site's Edsurn              |
        /// | 31028       | Invalid Contact Name                        |
        /// | 31029       | Invalid Contact Email                       |
        /// | 31030       | Invalid Contact Number                      |
        /// | 31031       | Invalid Training Type                       |
        /// | 31032       | Invalid Training Code                       |
        /// | 31033       | Invalid Desired skills                      |
        /// | 31034       | Invalid Desired personal qualities          |
        /// | 31035       | Invalid Desired qualifications              |
        /// | 31036       | Invalid Future prospects                    |
        /// | 31037       | Invalid Things to consider                  |
        /// | 31038       | Invalid Training to be provided             |
        /// | 31039       | Invalid Application method                  |
        /// | 31040       | Invalid Supplementary question 1            |
        /// | 31041       | Invalid Supplementary question 2            |
        /// | 31042       | Invalid External application url            |
        /// | 31043       | Invalid External Application Instructions   |
        /// | 31044       | Invalid Is Employer Disability Confident    |
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
