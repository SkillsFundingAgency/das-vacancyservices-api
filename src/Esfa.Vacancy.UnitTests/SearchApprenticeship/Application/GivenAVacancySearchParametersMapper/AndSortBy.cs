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
        public void ThenAssignsValueFromCalculateSortBy()
        {
            var request = new SearchApprenticeshipVacanciesRequest
                { SortBy = SortBy.Distance };

            var result = VacancySearchParametersMapper.Convert(request);

            result.SortBy.Should().Be(request.CalculateSortBy());
        }
    }
}