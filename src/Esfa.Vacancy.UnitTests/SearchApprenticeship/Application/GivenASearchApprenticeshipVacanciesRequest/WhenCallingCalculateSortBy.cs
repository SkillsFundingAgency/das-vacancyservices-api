using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequest
{
    [TestFixture]
    public class WhenCallingCalculateSortBy
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void AndIsAge_ThenReturnsAge()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.Age };

            request.CalculateSortBy().Should().Be(SortBy.Age);
        }

        [Test]
        public void AndIsDistance_ThenReturnsDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.Distance };

            request.CalculateSortBy().Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndLatitudeHasValue_ThenReturnsDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { Latitude = _fixture.Create<double>() };

            request.CalculateSortBy().Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndLongitudeHasValue_ThenMapsToDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { Longitude = _fixture.Create<double>() };

            request.CalculateSortBy().Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndDistanceInMilesHasValue_ThenReturnsDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { DistanceInMiles = _fixture.Create<int>() };

            request.CalculateSortBy().Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndNotLocationSearch_ThenReturnsAge()
        {
            var result = new SearchApprenticeshipVacanciesRequest();

            result.CalculateSortBy().Should().Be(SortBy.Age);
        }
    }
}