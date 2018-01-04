using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndPostedInLastNumberOfDays
    {
        private readonly List<string> _standardCodes = new List<string> { "9" };
        private VacancySearchParametersMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapper = fixture.Create<VacancySearchParametersMapper>();
        }

        [TestCase(2, TestName = "And PostedInLastNumberOfDays is 2 Then FromDate should be 2 days ago")]
        [TestCase(null, TestName = "And PostedInLastNumberOfDays is null Then FromDate should be null")]
        [TestCase(0, TestName = "And PostedInLastNumberOfDays is 0 Then FromDate should be today")]
        public void WhenPopulatingFromDate(int? sinceDays)
        {
            var expectedFromDate = sinceDays.HasValue
                ? DateTime.Today.AddDays(-sinceDays.Value)
                : (DateTime?)null;

            var result = _mapper.Convert(new SearchApprenticeshipVacanciesRequest()
                { PostedInLastNumberOfDays = sinceDays, StandardLarsCodes = _standardCodes });

            result.FromDate.Should().Be(expectedFromDate);
        }
    }
}
