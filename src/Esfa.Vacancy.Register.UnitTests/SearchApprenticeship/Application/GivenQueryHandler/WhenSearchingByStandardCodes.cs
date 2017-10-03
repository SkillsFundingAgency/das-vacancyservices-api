using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenQueryHandler
{
    [TestFixture]
    public class WhenSearchingByStandardCodes
    {
        private SearchApprenticeshipVacanciesQueryHandler _handler;
        private readonly Mock<IValidator<SearchApprenticeshipVacanciesRequest>> _mockValidator = new Mock<IValidator<SearchApprenticeshipVacanciesRequest>>();
        private readonly Mock<IVacancySearchService> _mockSearchService = new Mock<IVacancySearchService>();
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

            _handler = new SearchApprenticeshipVacanciesQueryHandler(
                _mockValidator.Object, _mockSearchService.Object, _mockRepository.Object);
        }

        [Test]
        public async Task AndStandardCodesAreValid_ThenPerformSearch()
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            { StandardCodes = new List<string>() { "100", "200" } };
            _mockValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult(new List<ValidationFailure>()));

            var expectedResponse = new SearchApprenticeshipVacanciesResponse();
            _mockSearchService.Setup(s => s.SearchApprenticeshipVacanciesAsync(It.IsAny<VacancySearchParameters>()))
                .ReturnsAsync(expectedResponse);

            var response = await _handler.Handle(request);

            response.Should().Be(expectedResponse);
        }

        [Test]
        public void AndAnyStandardCodeIsInvalid_ThenThrowValidationException()
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            { StandardCodes = new List<string>() { "100", "222" } };
            _mockValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult(new List<ValidationFailure>()));

            var expectedResponse = new SearchApprenticeshipVacanciesResponse();
            _mockSearchService.Setup(s => s.SearchApprenticeshipVacanciesAsync(It.IsAny<VacancySearchParameters>()))
                .ReturnsAsync(expectedResponse);

            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(request));

            Assert.IsTrue(exception.Errors.Any(ex => ex.ErrorMessage == "StandardCode 222 is invalid"));
        }
    }
}