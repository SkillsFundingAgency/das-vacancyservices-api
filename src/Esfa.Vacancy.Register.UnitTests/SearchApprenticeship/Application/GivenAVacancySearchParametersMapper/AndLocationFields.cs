using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndLocationFields
    {
        [Test]
        public void WhenMappingLongitude_ThenMappedToSearchParams()
        {
            var expectedLongitude = 35.23452344;
            var request = new SearchApprenticeshipVacanciesRequest
            {
                Longitude = expectedLongitude
            };

            var result = VacancySearchParametersMapper.Convert(request);

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

            var result = VacancySearchParametersMapper.Convert(request);

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

            var result = VacancySearchParametersMapper.Convert(request);

            result.DistanceInMiles.Should().Be(expectedDistance);
        }
    }
}