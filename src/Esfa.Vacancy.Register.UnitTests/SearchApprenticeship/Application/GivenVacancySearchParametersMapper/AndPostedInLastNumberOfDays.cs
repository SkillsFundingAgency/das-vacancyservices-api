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
        private readonly List<string> _standardCodes = new List<string> { "9" };

        [TestCase(2, TestName = "Then from date should be two days ago")]
        [TestCase(null, TestName = "Then from date should be null")]
        [TestCase(0, TestName = "Then from date should be today")]
        public async Task ThenPopulateFromDateAccordingly(int? sinceDays)
        {
            var expectedFromDate = sinceDays.HasValue
                ? DateTime.Today.AddDays(-sinceDays.Value)
                : (DateTime?)null;

            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest()
            { PostedInLastNumberOfDays = sinceDays, StandardCodes = _standardCodes });

            result.FromDate.Should().Be(expectedFromDate);
        }
    }
}
