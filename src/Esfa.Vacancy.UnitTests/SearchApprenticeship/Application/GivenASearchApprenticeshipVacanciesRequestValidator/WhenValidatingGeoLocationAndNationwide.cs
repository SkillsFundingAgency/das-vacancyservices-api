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
    public class WhenValidatingGeoLocationAndNationwide : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private const string ErrorText = "When searching by geo-location 'Latitude', 'Longitude' and 'DistanceInMiles' are required. You have not provided '{0}'.";

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 52.399085,
                    Longitude = -1.506115,
                    DistanceInMiles = 23,
                    NationwideOnly = true
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", ErrorMessages.SearchApprenticeships.GeoSearchAndNationwideNotAllowed)
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.NationwideOnly
                    }}
                })
                .SetName("And combining geo-location with nationwide only then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 52.399085,
                    NationwideOnly = true
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", ErrorMessages.SearchApprenticeships.GeoSearchAndNationwideNotAllowed)
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.NationwideOnly
                    }, new ValidationFailure("",  string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Longitude)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.Longitude
                    }, new ValidationFailure("",  string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.DistanceInMiles)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceInMiles
                    }}
                })
                .SetName("And combining latitude with nationwide only then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Longitude = 52.399085,
                    NationwideOnly = true
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", ErrorMessages.SearchApprenticeships.GeoSearchAndNationwideNotAllowed)
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.NationwideOnly
                    }, new ValidationFailure("",  string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Latitude)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.Latitude
                    }, new ValidationFailure("",  string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.DistanceInMiles)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceInMiles
                    }}
                })
                .SetName("And combining longitude with nationwide only then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    DistanceInMiles = 525,
                    NationwideOnly = true
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", ErrorMessages.SearchApprenticeships.GeoSearchAndNationwideNotAllowed)
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.NationwideOnly
                    }, new ValidationFailure("",  string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Latitude)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.Latitude
                    }, new ValidationFailure("",  string.Format(ErrorText, nameof(SearchApprenticeshipVacanciesRequest.Longitude)))
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.Longitude
                    }}
                })
                .SetName("And combining distance with nationwide only then invalid")
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