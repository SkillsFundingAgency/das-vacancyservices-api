using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingDistanceInMiles
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [Test]
        public void AndValueIsSet_ThenMapsToDistanceInMiles()
        {
            var expectedDistance = 349;
            var parameters = new SearchApprenticeshipParameters { DistanceInMiles = expectedDistance };
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.DistanceInMiles.Should().Be(expectedDistance);
        }

        [Test]
        public void AndValueIsNotSet_ThenMapsToNull()
        {
            var parameters = new SearchApprenticeshipParameters();
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.DistanceInMiles.Should().BeNull();
        }
    }
}