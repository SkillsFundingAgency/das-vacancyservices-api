using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndSortBy
    {
        [Test]
        public void ThenAssignsValueFromSortByCalculator()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            
            var expectedSortBy = (SortBy)234;
            var mockSortByCalculator = fixture.Freeze<Mock<ISortByCalculator>>();
            mockSortByCalculator
                .Setup(calculator => calculator.CalculateSortBy(It.IsAny<SearchApprenticeshipVacanciesRequest>()))
                .Returns(expectedSortBy);

            var mapper = fixture.Create<VacancySearchParametersMapper>();
            var result = mapper.Convert(new SearchApprenticeshipVacanciesRequest());

            result.SortBy.Should().Be(expectedSortBy);
        }
    }
}