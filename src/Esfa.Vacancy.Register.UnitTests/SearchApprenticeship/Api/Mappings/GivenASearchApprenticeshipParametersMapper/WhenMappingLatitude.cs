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
    public class WhenMappingLatitude
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
        public void AndValueIsSet_ThenMapsToLatitude()
        {
            var expectedLatitude = 52.787815;
            var parameters = new SearchApprenticeshipParameters { Latitude = expectedLatitude };
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.Latitude.Should().Be(expectedLatitude);
        }

        [Test]
        public void AndValueIsNotSet_ThenMapsToNull()
        {
            var parameters = new SearchApprenticeshipParameters();
            var result = _mapper.Map<SearchApprenticeshipVacanciesRequest>(parameters);
            result.Latitude.Should().BeNull();
        }
    }
}
