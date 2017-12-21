using AutoMapper;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchResponseMapper
{
    [TestFixture]
    public class WhenMappingSortBy
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [Test]
        public void ThenMapsAgeCorrectly()
        {
            var searchResponse = new SearchApprenticeshipVacanciesResponse { SortBy = SortBy.Age };
            var mapResult = _mapper.Map<Vacancy.Api.Types.SearchResponse<Vacancy.Api.Types.ApprenticeshipSummary>>(searchResponse);

            mapResult.SortBy.Should().Be(Vacancy.Api.Types.SortBy.Age);
        }

        [Test]
        public void ThenMapsDistanceCorrectly()
        {
            var searchResponse = new SearchApprenticeshipVacanciesResponse { SortBy = SortBy.Distance };
            var mapResult = _mapper.Map<Vacancy.Api.Types.SearchResponse<Vacancy.Api.Types.ApprenticeshipSummary>>(searchResponse);

            mapResult.SortBy.Should().Be(Vacancy.Api.Types.SortBy.Distance);
        }
    }
}