using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingSortBy : GivenSearchApprenticeshipVacanciesRequestValidatorBase
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
                    NationwideOnly = true,
                }, new ValidationResult())
                .SetName("Then null is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    PostedInLastNumberOfDays = 2,
                    NationwideOnly = true,
                    SortBy = SortBy.Age
                }, new ValidationResult())
                .SetName("And searching without location then sort by age is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 23,
                    Longitude = 54,
                    DistanceInMiles = 200,
                    SortBy = SortBy.Age
                }, new ValidationResult())
                .SetName("And searching with location then sort by age is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    PostedInLastNumberOfDays = 2,
                    NationwideOnly = true,
                    SortBy = SortBy.Distance
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("SortBy", ErrorMessages.SearchApprenticeships.SortByDistanceOnlyWhenGeoSearch)
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.SortByDistanceOnlyWhenGeoSearch
                    }}
                })
                .SetName("And searching without location then sort by distance is invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 23,
                    Longitude = 54,
                    DistanceInMiles = 200,
                    SortBy = SortBy.Distance
                }, new ValidationResult())
                .SetName("And searching with location then sort by distance is valid")
        };
    }
}
