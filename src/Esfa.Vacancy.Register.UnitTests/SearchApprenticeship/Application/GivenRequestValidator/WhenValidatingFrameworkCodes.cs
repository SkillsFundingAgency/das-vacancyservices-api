using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenRequestValidator
{
    [TestFixture]
    public class WhenValidatingFrameworkCodes
    {
        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingIsValid(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var validator = new SearchApprenticeshipVacanciesRequestValidator();

            var actualResult = validator.Validate(searchRequest);

            actualResult.IsValid.Should().Be(expectedResult.IsValid);
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingErrorMessages(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var validator = new SearchApprenticeshipVacanciesRequestValidator();

            var actualResult = validator.Validate(searchRequest);

            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorMessage));
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkCodes = new List<string> {"e"}
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("FrameworkCodes[0]", "e is invalid, expected a number.") }
                })
                .SetName("Then chars are not allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkCodes = new List<string> {"1.1"}
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("FrameworkCodes[0]", "1.1 is invalid, expected a number.") }
                })
                .SetName("Then floats are not allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkCodes = new List<string> {"6 2"}
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("FrameworkCodes[0]", "6 2 is invalid, expected a number.") }
                })
                .SetName("Then inner spaces are not allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkCodes = new List<string> {"  5345   "}
                }, new ValidationResult())
                .SetName("Then outer spaces are allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkCodes = new List<string> {"123424"}
                }, new ValidationResult())
                .SetName("Then ints are allowed")
        };
    }
}