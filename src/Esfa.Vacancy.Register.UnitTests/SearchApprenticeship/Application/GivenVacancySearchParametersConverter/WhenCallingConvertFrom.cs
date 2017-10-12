using System;
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

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenVacancySearchParametersConverter
{
    [TestFixture]
    public class WhenCallingConvertFrom
    {
        private VacancySearchParametersConverter _converter;
        private Mock<IFrameworkCodeConverter> _mockFrameworkConverter;
        private Mock<IStandardCodeConverter> _mockStandardCodeConverter;
        private List<string> _expectedStandards;
        private List<string> _expectedFrameworks;

        [SetUp]
        public void Setup()
        {
            _expectedStandards = new List<string> {"STDSEC.9", "STDSEC.3", "STDSEC.8"};
            _expectedFrameworks = new List<string> {"FW.343455", "FW.3434490"};

            _mockFrameworkConverter = new Mock<IFrameworkCodeConverter>();
            _mockFrameworkConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult());

            _mockStandardCodeConverter = new Mock<IStandardCodeConverter>();
            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult());

            _converter = new VacancySearchParametersConverter(_mockStandardCodeConverter.Object, _mockFrameworkConverter.Object);
        }

        [Test]
        public async Task ThenTheStandardCodesAreReturned()
        {
            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult{SubCategoryCodes = _expectedStandards});

            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(_expectedStandards);
        }

        [Test]
        public void AndStandardCodeValidationsAreReturned_ThenThrowsValidationException()
        {
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("StandardCode", Guid.NewGuid().ToString())
            };

            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult { ValidationFailures = validationFailures });

            var action = new Func<Task<VacancySearchParameters>>(() => _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest()));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {validationFailures[0].ErrorMessage}");
        }

        [Test]
        public async Task ThenTheFrameworkCodesAreReturned()
        {
            _mockFrameworkConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult { SubCategoryCodes = _expectedFrameworks });

            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(_expectedFrameworks);
        }

        [Test]
        public async Task AndFrameworkCodesAsWellAsStandardCodes_ThenBothAreReturned()
        {
            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult { SubCategoryCodes = _expectedStandards });

            _mockFrameworkConverter
                .Setup(converter => converter.ConvertAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult{SubCategoryCodes = _expectedFrameworks});

            var expectedSubCategoryCodes = new List<string>();
            expectedSubCategoryCodes.AddRange(_expectedStandards);
            expectedSubCategoryCodes.AddRange(_expectedFrameworks);

            var result = await _converter.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.SubCategoryCodes.ForEach(Console.WriteLine);

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(expectedSubCategoryCodes);
        }
    }
}