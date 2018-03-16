using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingFrameworkCodes : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private static List<TestCaseData> SuccessTestCases => new List<TestCaseData>
        {
            new TestCaseData(ValidFrameworkCodes)
                .SetName("Then any number is valid"),
        };

        [TestCaseSource(nameof(SuccessTestCases))]
        public void ThenPassValidation(List<string> input)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                FrameworkLarsCodes = input,
                StandardLarsCodes = new List<string>()
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        private static List<TestCaseData> FailingTestCases => new List<TestCaseData>
        {
            new TestCaseData("e",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, "e"),
                ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .SetName("Then characters are not allowed"),
            new TestCaseData("1.1",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, "1.1"),
                ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .SetName("Then decimals are not allowed"),
            new TestCaseData("2 0",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, "2 0"),
                ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .SetName("Then inner spaces are not allowed"),
            new TestCaseData("2",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeNotFoundErrorMessage(TrainingType.Framework,"2"),
                ErrorCodes.SearchApprenticeships.FrameworkCodeNotFound)
                .SetName("Then it should be a valid Framework code")
        };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ThenFailValidation(string input, string expectedErrorMessage, string expectedErrorCode)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                FrameworkLarsCodes = input.Split(',').ToList()
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
            result.Errors.First().ErrorMessage.Should().Be(expectedErrorMessage);
            result.Errors.First().ErrorCode.Should().Be(expectedErrorCode);
        }
    }
}