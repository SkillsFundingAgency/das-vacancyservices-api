using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndSortBy
    {
        [Test]
        public void AndIsAge_ThenMapsToAge()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.Age });

            result.SortBy.Should().Be(SortBy.Age);
        }

        [Test]
        public void AndIsDistance_ThenMapsToDistance()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.Distance });

            result.SortBy.Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndLatitudeHasValue_ThenMapsToDistance()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { Latitude = 34 });

            result.SortBy.Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndLongitudeHasValue_ThenMapsToDistance()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { Longitude = 34 });

            result.SortBy.Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndDistanceInMilesHasValue_ThenMapsToDistance()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { DistanceInMiles = 34 });

            result.SortBy.Should().Be(SortBy.Distance);
        }

        [Test]
        public void AndIsNullAndNotLocationSearch_ThenMapsToAge()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest());

            result.SortBy.Should().Be(SortBy.Age);
        }
    }
}