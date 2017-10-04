using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenRequestValidator
{
    public class WhenValidatingPageNumber
    {
        private SearchApprenticeshipVacanciesRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new SearchApprenticeshipVacanciesRequestValidator();
        }

        [TestCase(0, false, "Anything less than one is invalid")]
        [TestCase(1, true, "Minimum of one")]
        [TestCase(22, true, "Anything more than one is valid")]
        public void ThenValidate(int pageNumber, bool isValid, string reason)
        {
            var request = new SearchApprenticeshipVacanciesRequest { StandardCodes = new[] { "1" }, PageNumber = pageNumber };

            var result = _validator.Validate(request);

            result.IsValid.Should().Be(isValid, reason);
        }
    }
}
