using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequest
{
    [TestFixture]
    public class WhenGettingIsGeoSearch
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void AndNoGeoSearchFields_ThenFalse()
        {
            var request = new SearchApprenticeshipVacanciesRequest();

            request.IsGeoSearch.Should().BeFalse();
        }

        [Test]
        public void AndLatitudeOnly_ThenTrue()
        {
            var request = new SearchApprenticeshipVacanciesRequest
            {
                Latitude = _fixture.Create<double>()
            };

            request.IsGeoSearch.Should().BeTrue();
        }

        [Test]
        public void AndLongitudeOnly_ThenTrue()
        {
            var request = new SearchApprenticeshipVacanciesRequest
            {
                Longitude = _fixture.Create<double>()
            };

            request.IsGeoSearch.Should().BeTrue();
        }

        [Test]
        public void AndDistanceOnly_ThenTrue()
        {
            var request = new SearchApprenticeshipVacanciesRequest
            {
                DistanceInMiles = _fixture.Create<int>()
            };

            request.IsGeoSearch.Should().BeTrue();
        }
    }
}