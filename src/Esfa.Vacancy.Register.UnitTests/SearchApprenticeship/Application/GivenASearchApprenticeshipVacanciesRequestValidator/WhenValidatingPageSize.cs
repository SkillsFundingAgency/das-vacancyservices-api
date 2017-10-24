using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingPageSize
    {
        private SearchApprenticeshipVacanciesRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new SearchApprenticeshipVacanciesRequestValidator();
        }

        [TestCase(0, false, "Anything less than one is invalid")]
        [TestCase(1, true, "Minimum of one")]
        [TestCase(22, true, "It should be between 1 and 250")]
        [TestCase(250, true, "Maximum allowed is 250")]
        [TestCase(251, false, "Anything more than 250 is invalid")]
        public void ThenValidate(int pageSize, bool isValid, string reason)
        {
            var request = new SearchApprenticeshipVacanciesRequest { StandardCodes = new List<string> { "1" }, PageSize = pageSize };

            var result = _validator.Validate(request);

            result.IsValid.Should().Be(isValid, reason);
        }
    }
}
