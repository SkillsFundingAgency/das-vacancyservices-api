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
    public class WhenValidatingDistanceInMiles : GivenSearchApprenticeshipVacanciesRequestValidatorBase
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
                .SetName("And greater than 0 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 45,
                    Longitude = -18,
                    DistanceInMiles = 0
                }, new ValidationResult())
                .SetName("And 0 then valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardLarsCodes = ValidStandardCodes,
                    Latitude = 45,
                    Longitude = -18,
                    DistanceInMiles = -1
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("", "'Distance In Miles' must be greater than or equal to '0'.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.DistanceLessThan0
                    }}
                })
                .SetName("And less than 0 then invalid")
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