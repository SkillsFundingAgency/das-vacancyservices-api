using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndLocationFields
    {
        private VacancySearchParametersMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapper = fixture.Create<VacancySearchParametersMapper>();
        }

        [Test]
        public void WhenMappingLongitude_ThenMappedToSearchParams()
        {
            var expectedLongitude = 35.23452344;
            var request = new SearchApprenticeshipVacanciesRequest
            {
                Longitude = expectedLongitude
            };

            var result = _mapper.Convert(request);

            result.Longitude.Should().Be(expectedLongitude);
        }

        [Test]
        public void WhenMappingLatitude_ThenMappedToSearchParams()
        {
            var expectedLatitude = -0.9083434;
            var request = new SearchApprenticeshipVacanciesRequest
            {
                Latitude = expectedLatitude
            };

            var result = _mapper.Convert(request);

            result.Latitude.Should().Be(expectedLatitude);
        }

        [Test]
        public void WhenMappingDistanceInMiles_ThenMappedToSearchParams()
        {
            var expectedDistance = 9958;
            var request = new SearchApprenticeshipVacanciesRequest
            {
                DistanceInMiles = expectedDistance
            };

            var result = _mapper.Convert(request);

            result.DistanceInMiles.Should().Be(expectedDistance);
        }
    }
}