using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenSearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingPostedInDays : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        [TestCase(-1, false, "Posted in days should be 0 or more")]
        [TestCase(0, true, "Posted in days should be 0 or more")]
        [TestCase(1, true, "Posted in days should be 0 or more")]
        public void TestValidation(int postedInDays, bool isValid, string reason)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                FrameworkCodes = new List<string> { "1" },
                PostedInLastNumberOfDays = postedInDays
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().Be(isValid, reason);
        }
    }
}
