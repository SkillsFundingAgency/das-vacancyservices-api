using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersBuilder
{
    [TestFixture]
    public class AndPostedInDays
    {
        private VacancySearchParametersBuilder _builder;
        private Mock<IFrameworkCodeConverter> _mockFrameworkConverter;
        private Mock<IStandardCodeConverter> _mockStandardCodeConverter;
        private List<string> _expectedStandards;

        [SetUp]
        public void Setup()
        {
            _expectedStandards = new List<string> { "STDSEC.9", "STDSEC.3", "STDSEC.8" };

            _mockFrameworkConverter = new Mock<IFrameworkCodeConverter>();
            _mockFrameworkConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult());

            _mockStandardCodeConverter = new Mock<IStandardCodeConverter>();
            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult());

            _builder = new VacancySearchParametersBuilder(_mockStandardCodeConverter.Object, _mockFrameworkConverter.Object);
        }
        [Test]
        public async Task ThenReturnFromDateAccordingly()
        {
            var result = await _builder.BuildAsync(new SearchApprenticeshipVacanciesRequest()
            { PostedInDays = 2, StandardCodes = _expectedStandards });

            result.FromDate.Should().Be(DateTime.Today.AddDays(-2), "From date is these many days ahead from today");
        }

        [Test]
        public async Task ThenReturnNullFromDate()
        {
            var result = await _builder.BuildAsync(new SearchApprenticeshipVacanciesRequest()
            { PostedInDays = 0, StandardCodes = _expectedStandards });

            result.FromDate.Should().BeNull("Value should be greater than zero");
        }
    }
}
