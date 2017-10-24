using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingMinimumRequiredFields
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
                    FrameworkCodes = new List<string> {"2345"}
                }, new ValidationResult())
                .SetName("Frameworks present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new List<string> {"2345"}
                }, new ValidationResult())
                .SetName("Standards present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    FrameworkCodes = new List<string> {"34"},
                    StandardCodes = new List<string> {"768657"}
                }, new ValidationResult())
                .SetName("Frameworks and Standards present is allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest(), new ValidationResult
                {
                    Errors =
                    {
                        new ValidationFailure("", "At least one of StandardCodes or FrameworkCodes is required.")
                    }
                })
                .SetName("No Frameworks or Standards present is not allowed")
        };
    }
}