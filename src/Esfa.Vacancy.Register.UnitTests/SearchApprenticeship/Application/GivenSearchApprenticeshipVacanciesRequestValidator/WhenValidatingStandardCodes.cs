using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenSearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingStandardCodes : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        [TestCase("", false, 1, "Empty strings are expected to be handled by consumer")]
        [TestCase("10,20", true, 0, "Any number is valid")]
        [TestCase("10 , 20", true, 0, "Leading and preceeding spaces are allowed with numbers")]
        [TestCase("10 , 2 0, 1e", false, 2, "Spaces with in value and alphabets are not accepted")]
        [TestCase(" 10 ", true, 0, "One number is valid")]
        public void ThenValidate(string input, bool expectedOutput, int expectedNumberOfErrors, string message)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                StandardCodes = input.Split(',')
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().Be(expectedOutput);

            result.Errors.Count.ShouldBeEquivalentTo(expectedNumberOfErrors);
        }

        [Test]
        public void AndNullStandardCodes_ThenFailValidation()
        {
            var request = new SearchApprenticeshipVacanciesRequest { StandardCodes = null };
            var result = Validator.Validate(request);
            result.IsValid.Should().BeFalse();
        }

    }
}
