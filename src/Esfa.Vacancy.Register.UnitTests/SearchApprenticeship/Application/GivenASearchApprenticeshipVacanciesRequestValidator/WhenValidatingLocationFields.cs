using System;
using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingLocationFields : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 52.399085,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult()) // IsValid == true when Errors collection is empty
                .SetName("And all fields present then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 52.399085,
                    Longitude = -1.506115
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Distance In Miles' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceMissingFromGeoSearch
                    }}
                })
                .SetName("And no distance then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Longitude = -1.506115,
                    DistanceInMiles = 342
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Latitude' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch
                    }}
                })
                .SetName("And no latitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 52.399085,
                    DistanceInMiles = 342
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Longitude' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LongitudeMissingFromGeoSearch
                    }}
                })
                .SetName("And no longitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 52.399085
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Longitude' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LongitudeMissingFromGeoSearch
                    },new ValidationFailure("", "'Distance In Miles' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceMissingFromGeoSearch
                    }}
                })
                .SetName("And only latitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Longitude = -1.506115
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Latitude' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch
                    },new ValidationFailure("", "'Distance In Miles' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceMissingFromGeoSearch
                    }}
                })
                .SetName("And only longitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    DistanceInMiles = 342
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Latitude' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch
                    },new ValidationFailure("", "'Longitude' must not be empty.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LongitudeMissingFromGeoSearch
                    }}
                })
                .SetName("And only distance then invalid")
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