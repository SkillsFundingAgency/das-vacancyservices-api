using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingLongitude : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 52.399085,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult())
                .SetName("And between -180 and 180 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = -180,
                    DistanceInMiles = 235
                }, new ValidationResult())
                .SetName("And -180 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = 180,
                    DistanceInMiles = 235
                }, new ValidationResult())
                .SetName("And 180 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = -180.1,
                    DistanceInMiles = 235
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Longitude' must be between -180 and 180. You entered -180.1.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LongitudeOutsideRange
                    }}
                })
                .SetName("And less than -180 then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = 180.1,
                    DistanceInMiles = 235
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Longitude' must be between -180 and 180. You entered 180.1.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LongitudeOutsideRange
                    }}
                })
                .SetName("And greater than 180 then invalid")
        };

        [TestCaseSource(nameof(TestCases))]
        public void AndRunningTestCases(SearchApprenticeshipVacanciesRequest request, ValidationResult expectedResult)
        {
            var actualResult = Validator.Validate(request);

            foreach (var validationFailure in actualResult.Errors)
            {
                Console.WriteLine(validationFailure.ErrorMessage);
            }

            actualResult.IsValid.Should().Be(expectedResult.IsValid);
            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorMessage)
                    .Including(failure => failure.ErrorCode));
        }
    }
}