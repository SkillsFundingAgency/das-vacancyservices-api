using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture, Ignore("not complete")]
    public class AndSortOrder
    {
        [Test]
        public void AndIsAge_ThenMapsToAge()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { SortOrder = SortOrder.Age });

            result.SortOrder.Should().Be("todo");
        }

        [Test]
        public void AndIsDistance_ThenMapsToDistance()
        {

        }

        [Test]
        public void AndIsNullAndLocationSearch_ThenMapsToDistance()
        {

        }

        [Test]
        public void AndIsNullAndNotLocationSearch_ThenMapsToAge()
        {

        }
    }
}