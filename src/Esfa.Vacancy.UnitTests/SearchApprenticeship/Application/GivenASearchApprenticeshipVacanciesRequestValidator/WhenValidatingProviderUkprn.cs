using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    public class WhenValidatingProviderUkprn : GivenSearchApprenticeshipVacanciesRequestValidatorBase
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
                .SetName("Then default is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    Ukprn = 88888888
                }, new ValidationResult())
                .SetName("Then 8 digits is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    Ukprn = 8888888
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("Ukprn", "UKPRN must be 8 digits in length")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.Ukprn
                    }}
                })
                .SetName("Then less than 8 is invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    Ukprn = 888888888
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("Ukprn", "UKPRN must be 8 digits in length")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.Ukprn
                    }}
                })
                .SetName("Then greater than 8 is invalid"),
        };
    }
}
