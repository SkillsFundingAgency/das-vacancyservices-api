using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndPostedInLastNumberOfDays
    {
        private readonly List<string> _standardCodes = new List<string> { "9" };

        [TestCase(2, TestName = "And PostedInLastNumberOfDays is 2 Then FromDate should be 2 days ago")]
        [TestCase(null, TestName = "And PostedInLastNumberOfDays is null Then FromDate should be null")]
        [TestCase(0, TestName = "And PostedInLastNumberOfDays is 0 Then FromDate should be today")]
        public void WhenPopulatingFromDate(int? sinceDays)
        {
            var expectedFromDate = sinceDays.HasValue
                ? DateTime.Today.AddDays(-sinceDays.Value)
                : (DateTime?)null;

            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest()
                { PostedInLastNumberOfDays = sinceDays, StandardLarsCodes = _standardCodes });

            result.FromDate.Should().Be(expectedFromDate);
        }
    }
}
