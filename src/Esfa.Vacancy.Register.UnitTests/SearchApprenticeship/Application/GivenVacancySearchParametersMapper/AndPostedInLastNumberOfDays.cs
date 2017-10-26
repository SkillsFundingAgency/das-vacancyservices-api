using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenVacancySearchParametersMapper
{
    [TestFixture]
    public class AndPostedInLastNumberOfDays
    {
        private List<string> _expectedStandards = new List<string> { "STDSEC.9", "STDSEC.3", "STDSEC.8" };
        [Test]
        public async Task ThenReturnFromDateAccordingly()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest()
            { PostedInLastNumberOfDays = 2, StandardCodes = _expectedStandards });

            result.FromDate.Should().Be(DateTime.Today.AddDays(-2), "From date is that many days ahead from today");
        }

        [Test]
        public async Task ThenReturnNullFromDate()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest()
            { PostedInLastNumberOfDays = 0, StandardCodes = _expectedStandards });

            result.FromDate.Should().BeNull("Value should be greater than zero");
        }
    }
}
