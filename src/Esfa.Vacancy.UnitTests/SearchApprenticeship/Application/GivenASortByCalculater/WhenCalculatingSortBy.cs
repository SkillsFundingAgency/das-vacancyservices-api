using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASortByCalculater
{
    [TestFixture]
    public class WhenCalculatingSortBy
    {
        private Fixture _fixture;
        private SortByCalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _calculator = _fixture.Create<SortByCalculator>();
        }

        [Test]
        public void AndIsAge_ThenMapsToAge()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.Age.ToString() };

            _calculator.CalculateSortBy(request).Should().Be(SortBy.Age);
        }

        [Test]
        public void AndIsDistance_ThenMapsToDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.Distance.ToString() };

            _calculator.CalculateSortBy(request).Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsExpectedStartDate_ThenMapsToExpectedStartDate()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.ExpectedStartDate.ToString() };

            _calculator.CalculateSortBy(request).Should().Be(SortBy.ExpectedStartDate);
        }

        [Test]
        public void AndIsNullAndLatitudeHasValue_ThenMapsToDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { Latitude = _fixture.Create<double>() };

            _calculator.CalculateSortBy(request).Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndLongitudeHasValue_ThenMapsToDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { Longitude = _fixture.Create<double>() };

            _calculator.CalculateSortBy(request).Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndDistanceInMilesHasValue_ThenMapsToDistance()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { DistanceInMiles = _fixture.Create<int>() };

            _calculator.CalculateSortBy(request).Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndNotLocationSearch_ThenMapsToAge()
        {
            var request = new SearchApprenticeshipVacanciesRequest();

            _calculator.CalculateSortBy(request).Should().Be(SortBy.Age);
        }
    }
}