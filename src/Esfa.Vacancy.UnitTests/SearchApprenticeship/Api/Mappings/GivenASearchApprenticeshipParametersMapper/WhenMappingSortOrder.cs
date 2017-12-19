using AutoMapper;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Api;
using ApiTypes = Esfa.Vacancy.Api.Types;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingSortOrder
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [Test]
        public void WhenAge_ThenSetToAge()
        {
            var parameters = new ApiTypes.SearchApprenticeshipParameters() { SortBy = ApiTypes.SortBy.Age };
            var request = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            request.SortBy.Should().Be(SortBy.Age);
        }

        [Test]
        public void WhenDistance_ThenSetToDistance()
        {
            var parameters = new ApiTypes.SearchApprenticeshipParameters() { SortBy = ApiTypes.SortBy.Distance };
            var request = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            request.SortBy.Should().Be(SortBy.Distance);
        }

        [Test]
        public void WhenNull_ThenSetToNull()
        {
            var parameters = new ApiTypes.SearchApprenticeshipParameters() { SortBy = null };
            var request = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            request.SortBy.Should().BeNull();
        }
    }
}