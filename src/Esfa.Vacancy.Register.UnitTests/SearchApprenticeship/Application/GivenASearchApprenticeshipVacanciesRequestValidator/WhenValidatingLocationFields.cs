using System;
using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingLocationFields : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        [Test]
        public void AndAllThreeFieldsArePresentAndEachFieldIsValid_ThenRequestIsValid()
        {
            var result = Validator.Validate(new SearchApprenticeshipVacanciesRequest
            {
                StandardLarsCodes = ValidStandardCodes,
                Latitude = 52.399085,
                Longitude = -1.506115,
                DistanceInMiles = 342
            });

            result.IsValid.Should().BeTrue();
        }

        private static List<TestCaseData> FailingTestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Latitude = 52.399085,
                        Longitude = -1.506115
                    },
                "'Distance In Miles' must not be empty.",
                ErrorCodes.SearchApprenticeships.DistanceMissingFromGeoSearch)
                .SetName("And no distance then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Longitude = -1.506115,
                        DistanceInMiles = 342
                    },
                    "'Latitude' must not be empty.",
                ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch)
                .SetName("And no latitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Latitude = 52.399085,
                        DistanceInMiles = 342
                    },
                    "'Longitude' must not be empty.",
                ErrorCodes.SearchApprenticeships.LongitudeMissingFromGeoSearch)
                .SetName("And no longitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Latitude = 52.399085
                    },
                    "'Longitude' must not be empty.",
                    ErrorCodes.SearchApprenticeships.LongitudeMissingFromGeoSearch)
                .SetName("And only latitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Longitude = -1.506115
                    },
                    "'Latitude' must not be empty.",
                    ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch)
                .SetName("And only longitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        DistanceInMiles = 342
                    },
                    "'Latitude' must not be empty.",
                    ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch)
                .SetName("And only distance then invalid")
        };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ValidateMissingFields(SearchApprenticeshipVacanciesRequest request, string expectedErrorMessage, string expectedErrorCode)
        {
            var result = Validator.Validate(request);

            foreach (var validationFailure in result.Errors)
            {
                Console.WriteLine(validationFailure.ErrorMessage);
            }

            result.IsValid.Should().Be(false);
            
            result.Errors.First().ErrorMessage.Should().Be(expectedErrorMessage);
            result.Errors.First().ErrorCode.Should().Be(expectedErrorCode);
        }
    }
}