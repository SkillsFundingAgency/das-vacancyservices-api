using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndNationwideOnly
    {
        [Test]
        public void AndIsTrue_ThenMapsToNationwide()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = true });

            result.LocationType.Should().Be(VacancySearchParametersMapper.NationwideLocationType);
        }

        [Test]
        public void AndIsTrue_ThenGeoCodeFieldsAreSetToNull()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = true, Latitude = 1, Longitude = 1, DistanceInMiles = 1});

            result.Latitude.Should().BeNull();
            result.Longitude.Should().BeNull();
            result.DistanceInMiles.Should().BeNull();
        }

        [Test]
        public void AndIsFalse_ThenMapsToNonNationwide()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = false });

            result.LocationType.Should().Be(VacancySearchParametersMapper.NonNationwideLocationType);
        }
    }
}