using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenSearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingStandardCodes : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        public static List<TestCaseData> FailingTestCases => new List<TestCaseData>
        {
            new TestCaseData("1e", "1e is invalid, expected a number.")
                .SetName("Then alphabets are not accepted"),
            new TestCaseData("2 0", "2 0 is invalid, expected a number.")
                .SetName("Then spaces with in value are not accepted"),
            new TestCaseData("2", "Standard code 2 is invalid.")
                .SetName("Then it should be a valid standard code")
        };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ThenFailValidation(string input, string expectedErrorMessage)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                StandardCodes = input.Split(',').ToList()
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
            result.Errors.First().ErrorMessage.Should().Be(expectedErrorMessage);
        }

        [Test]
        public void AndNullStandardCodes_ThenFailValidation()
        {
            var request = new SearchApprenticeshipVacanciesRequest { StandardCodes = null };
            var result = Validator.Validate(request);
            result.IsValid.Should().BeFalse();
        }

        public static List<TestCaseData> SuccessTestCases => new List<TestCaseData>
        {
            new TestCaseData(ValidStandardCodes)
                .SetName("Then any number is valid"),
            new TestCaseData(" 1 ".Split(',').ToList())
                .SetName("Then leading and preceeding spaces are allowed with numbers"),
        };

        [TestCaseSource(nameof(SuccessTestCases))]
        public void ThenPassValidation(List<string> input)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                StandardCodes = input
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().Be(true);
        }
    }
}
