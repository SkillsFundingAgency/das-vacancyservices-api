using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenRequestValidator
{
    [TestFixture]
    public class WhenValidatingPostedInDays
    {
        [TestCase(-1, false, "Posted in days should be 0 or more")]
        [TestCase(0, true, "Posted in days should be 0 or more")]
        [TestCase(1, true, "Posted in days should be 0 or more")]
        public void TestValidation(int postedInDays, bool isValid, string reason)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                FrameworkCodes = new[] { "1" },
                PostedInDays = postedInDays
            };

            var validator = new SearchApprenticeshipVacanciesRequestValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().Be(isValid, reason);
        }
    }
}
