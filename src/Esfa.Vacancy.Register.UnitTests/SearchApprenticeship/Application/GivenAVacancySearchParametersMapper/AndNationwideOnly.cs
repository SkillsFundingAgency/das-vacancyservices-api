using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    public class AndNationwideOnly
    {
        [Test]
        public void AndIsTrue_ThenMapsToNationwide()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = true });

            result.LocationType.Should().Be("National");
        }

        [Test]
        public void AndIsFalse_ThenMapsToNull()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = false });

            result.LocationType.Should().BeNull();
        }
    }
}