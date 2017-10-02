using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.Register.UnitTests.Application.Queries.SearchApprenticeshipVacancies
{
    [TestFixture]
    public class SearchApprenticeshipVacanciesQueryHandlerTests
    {
        [Test]
        public async Task GivenStandardCodesResolveSectorCodesInSearchParameters()
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            { StandardCodes = new List<string>() { "1", "2" } };

            var sectorCodes = new List<string>() { "stdsec.10", "stdsec.10" };

            var expectedResponse = new SearchApprenticeshipVacanciesResponse();

            var mockValidator = new Mock<IValidator<SearchApprenticeshipVacanciesRequest>>();
            mockValidator.Setup(v => v.Validate(request)).Returns(new ValidationResult(new List<ValidationFailure>()));

            var mockSectorResolver = new Mock<IStandardSectorCodeResolver>();
            mockSectorResolver.Setup(r => r.ResolveAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(sectorCodes);

            var mockSearchService = new Mock<IVacancySearchService>();
            mockSearchService.Setup(s => s.SearchApprenticeshipVacanciesAsync(It.IsAny<VacancySearchParameters>()))
                .ReturnsAsync(expectedResponse);

            var handler = new SearchApprenticeshipVacanciesQueryHandler(
                mockValidator.Object, mockSearchService.Object, mockSectorResolver.Object);

            var result = await handler.Handle(request);

            result.Should().Be(expectedResponse);
        }
    }
}
