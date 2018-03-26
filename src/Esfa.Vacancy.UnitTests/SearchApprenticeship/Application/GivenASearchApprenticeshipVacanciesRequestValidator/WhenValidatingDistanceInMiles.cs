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
    public class WhenValidatingDistanceInMiles : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 52.399085,
                    Longitude = -1.506115,
                    DistanceInMiles = 235
                }, new ValidationResult())
                .SetName("And between 1 and 1000"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = -18,
                    DistanceInMiles = 1
                }, new ValidationResult())
                .SetName("And 1 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = -18,
                    DistanceInMiles = 1000
                }, new ValidationResult())
                .SetName("And 1000 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = -18,
                    DistanceInMiles = 0
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Distance In Miles' must be between 1 and 1000. You entered 0.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceInMiles
                    }}
                })
                .SetName("And less than 1 then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    Latitude = 45,
                    Longitude = -18,
                    DistanceInMiles = 1001
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Distance In Miles' must be between 1 and 1000. You entered 1001.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceInMiles
                    }}
                })
                .SetName("And greater than 1000 then invalid")
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