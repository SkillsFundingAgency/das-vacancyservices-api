using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.GivenSearchApprenticeshipParameters
{
    [TestFixture]
    public class AndPageNumber
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [Test]
        public void WhenProvided_ThenPopulateRequestWithTheGivenValue()
        {
            var expectedPageNumber = 2;
            var parameters = new SearchApprenticeshipParameters() { PageNumber = expectedPageNumber };
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageNumber.Should().Be(expectedPageNumber);
        }

        [Test]
        public void WhenNotProvided_ThenPoplateRequestWithTheDefaultValue()
        {
            var parameters = new SearchApprenticeshipParameters();
            var result = Mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageNumber.Should().Be(1);
        }
    }
}
