using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.App_Start;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.GivenSearchApprenticeshipParameters
{
    [TestFixture]
    public class AndPageSize
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [Test]
        public void WhenProvided_ThenPopulateRequestWithTheGivenValue()
        {
            var expectedPageSize = 2;
            var parameters = new SearchApprenticeshipParameters() { PageSize = expectedPageSize };
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageSize.Should().Be(expectedPageSize);
        }

        [Test]
        public void WhenNotProvided_ThenPoplateRequestWithTheDefaultValue()
        {
            var parameters = new SearchApprenticeshipParameters();
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageSize.Should().Be(100);
        }
    }
}
