using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
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

            var mockStandardCodeConverter = new Mock<IStandardCodeConverter>();
            mockStandardCodeConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new List<string>());

            var mockFrameworkConverter = new Mock<IFrameworkCodeConverter>();
            mockFrameworkConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(_expectedFrameworks);

            _converter = new VacancySearchParametersConverter(mockStandardCodeConverter.Object, mockFrameworkConverter.Object);
        }

        [Test]
        public async Task ThenTheFrameworkCodesAreReturned()
        {
            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(_expectedFrameworks);
        }
    }
}