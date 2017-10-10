using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenVacancySearchParametersConverter
{
    [TestFixture]
    public class WhenSearchingByStandardCodes
    {
        private VacancySearchParametersConverter _converter;
        private readonly Mock<IStandardRepository> _mockRepository = new Mock<IStandardRepository>();

        [SetUp]
        public void Setup()
        {
            var standardSectors = new List<StandardSector>()
            {
                new StandardSector() {LarsCode = 100, StandardSectorId = 1},
                new StandardSector() {LarsCode = 110, StandardSectorId = 1},
                new StandardSector() {LarsCode = 200, StandardSectorId = 2},
                new StandardSector() {LarsCode = 210, StandardSectorId = 2},
                new StandardSector() {LarsCode = 300, StandardSectorId = 3},
                new StandardSector() {LarsCode = 310, StandardSectorId = 3}
            };
            _mockRepository.Setup(r => r.GetStandardsAndRespectiveSectorIdsAsync()).ReturnsAsync(standardSectors);

            _converter = new VacancySearchParametersConverter(_mockRepository.Object);
        }

        [Test]
        public async Task ThenConvertToDistinctSectorCodes()
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            { StandardCodes = new List<string>() { "100", "110", "200", "210" } };

            var response = await _converter.ConvertFrom(request);

            response.SubCategoryCodes.Count().Should().Be(2);
            response.SubCategoryCodes.Should().Contain($"{StandardSector.StandardSectorPrefix}.{1}");
            response.SubCategoryCodes.Should().Contain($"{StandardSector.StandardSectorPrefix}.{2}");
        }

        [Test]
        public void AndAnyStandardCodeIsInvalid_ThenThrowValidationException()
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            { StandardCodes = new List<string>() { "100", "222" } };

            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _converter.ConvertFrom(request));

            Assert.IsTrue(exception.Errors.Any(ex => ex.ErrorMessage == "StandardCode 222 is invalid"));
        }

        [Test]
        public async Task AndStandardCodesIsEmpty_ThenReturnsEmpty()
        {
            var request = new SearchApprenticeshipVacanciesRequest();

            var result =  await _converter.ConvertFrom(request);

            result.SubCategoryCodes.Should().BeEmpty();
        }

        [Test]
        public async Task AndFrameworkCodesAsWellAsStandardCodes_ThenBothAreReturned()
        {
            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest
            {
                FrameworkCodes = new[] { "567", "234" },
                StandardCodes = new[] { "210", "310" }
            });

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(new[]
            {
                $"{StandardSector.StandardSectorPrefix}.2",
                $"{StandardSector.StandardSectorPrefix}.3",
                "FW.567",
                "FW.234"
            });
        }
    }
}