using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingGeoSearchFields : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private const string ErrorText = "When searching by geo-location 'Latitude', 'Longitude' and 'DistanceInMiles' are required. You have not provided '{0}'.";

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
                    Errors = { new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.DistanceInMiles)))
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
                    Errors = { new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Latitude)))
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
                    Errors = { new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Longitude)))
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
                    Errors = { new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Longitude)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LongitudeMissingFromGeoSearch
                    },new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.DistanceInMiles)))
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
                    Errors = { new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Latitude)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch
                    },new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.DistanceInMiles)))
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
                    Errors = { new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Latitude)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch
                    },new ValidationFailure("", string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Longitude)))
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