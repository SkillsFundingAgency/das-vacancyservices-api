using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingPageSize
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [Test]
        public void WhenProvided_ThenPopulateRequestWithTheGivenValue()
        {
            var expectedPageSize = 2;
            var parameters = new SearchApprenticeshipParameters() { PageSize = expectedPageSize };
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageSize.Should().Be(expectedPageSize);
        }

        [Test]
        public void WhenNotProvided_ThenPoplateRequestWithTheDefaultValue()
        {
            var parameters = new SearchApprenticeshipParameters();
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.PageSize.Should().Be(100);
        }
    }
}
