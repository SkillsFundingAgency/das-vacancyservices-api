using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingDistanceInMiles
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mockContext = new Mock<IContext>();
            MapperConfiguration config = AutoMapperConfig.Configure(mockContext.Object);
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
