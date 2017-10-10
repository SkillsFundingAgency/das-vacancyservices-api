using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenVacancySearchParametersConverter
{
    [TestFixture]
    public class WhenRequestContainsFrameworkCodes
    {
        private VacancySearchParametersConverter _converter;
        private readonly Mock<IStandardRepository> _mockRepository = new Mock<IStandardRepository>();

        [SetUp]
        public void Setup()
        {
            _mockRepository
                .Setup(r => r.GetStandardsAndRespectiveSectorIdsAsync())
                .ReturnsAsync(new List<StandardSector>());

            _converter = new VacancySearchParametersConverter(_mockRepository.Object);
        }

        [Test]
        public async Task AndFrameworkCodesHaveNoSpaces_ThenTheFrameworkCodesAreReturned()
        {
            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest
            {
                FrameworkCodes = new[] {"345", "3490"}
            });

            result.FrameworkCodes.ShouldAllBeEquivalentTo(new[]{"FW.345", "FW.3490"});
        }

        [Test]
        public async Task AndFrameworkCodesHaveSpaces_ThenTheFrameworkCodesAreReturned()
        {
            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest
            {
                FrameworkCodes = new[] { "43508 ", " 567 ", " 450" }
            });

            result.FrameworkCodes.ShouldAllBeEquivalentTo(new[] { "FW.43508", "FW.567", "FW.450" });
        }

        [Test]
        public async Task AndFrameworkCodesListEmpty_ThenEmptyListReturned()
        {
            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.FrameworkCodes.Should().BeEmpty();
        }

        [Test]
        public async Task AndFrameworkCodesAsWellAsStandardCodes_ThenBothAreReturned()
        {
            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest
            {
                FrameworkCodes = new[] { "43508 ", " 567 ", " 450" }
            });

            result.FrameworkCodes.ShouldAllBeEquivalentTo(new[] { "FW.43508", "FW.567", "FW.450" });
        }
    }
}