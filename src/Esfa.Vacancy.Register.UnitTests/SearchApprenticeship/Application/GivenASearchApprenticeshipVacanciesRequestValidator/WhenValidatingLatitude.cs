using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingLatitude : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 52.399085,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult())
                .SetName("And between -90 and 90 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = -90,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult())
                .SetName("And -90 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 90,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult())
                .SetName("And 90 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = -90.1,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Latitude' must be between -90 and 90. You entered -90.1.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LatitudeOutsideRange
                    }}
                })
                .SetName("And less than -90 then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 90.1,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Latitude' must be between -90 and 90. You entered 90.1.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LatitudeOutsideRange
                    }}
                })
                .SetName("And greater than 90 then invalid")
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