using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
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
        private List<string> _expectedFrameworks;

        [SetUp]
        public void Setup()
        {
            _expectedFrameworks = new List<string> { "FW.343455", "FW.3434490" };

            var mockRepository = new Mock<IStandardRepository>();
            mockRepository
                .Setup(r => r.GetStandardsAndRespectiveSectorIdsAsync())
                .ReturnsAsync(new List<StandardSector>());

            var mockFrameworkConverter = new Mock<IFrameworkCodeConverter>();
            mockFrameworkConverter
                .Setup(converter => converter.Convert(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(_expectedFrameworks);

            _converter = new VacancySearchParametersConverter(mockRepository.Object, mockFrameworkConverter.Object);
        }

        [Test]
        public async Task ThenTheFrameworkCodesAreReturned()
        {
            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(_expectedFrameworks);
        }
    }
}