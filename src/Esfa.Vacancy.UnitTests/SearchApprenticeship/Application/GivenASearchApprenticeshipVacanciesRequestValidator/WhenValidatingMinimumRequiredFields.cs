using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingMinimumRequiredFields : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingIsValid(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = Validator.Validate(searchRequest);

            actualResult.IsValid.Should().Be(expectedResult.IsValid);
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingErrorMessages(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = Validator.Validate(searchRequest);

            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorMessage));
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingErrorCodes(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = Validator.Validate(searchRequest);

            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorCode));
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkLarsCodes = ValidFrameworkCodes
                }, new ValidationResult())
                .SetName("Frameworks present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidFrameworkCodes
                }, new ValidationResult())
                .SetName("Standards present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkLarsCodes = ValidFrameworkCodes,
                    StandardLarsCodes = ValidStandardCodes
                }, new ValidationResult())
                .SetName("Frameworks and Standards present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true
                }, new ValidationResult())
                .SetName("Nationwide present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    PostedInLastNumberOfDays = 3242
                }, new ValidationResult())
                .SetName("PostedInLastNumberOfDays present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 23.2,
                    Longitude = 75.7,
                    DistanceInMiles = 76
                }, new ValidationResult())
                .SetName("Geo-Location fields present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest(), new ValidationResult
                {
                    Errors =
                    {
                        new ValidationFailure("", ErrorMessages.SearchApprenticeships.MinimumRequiredFieldsNotProvided)
                        {
                            ErrorCode = ErrorCodes.SearchApprenticeships.MinimumRequiredFieldsNotProvided
                        }
                    }
                })
                .SetName("No required fields present is not allowed")
        };
    }
}