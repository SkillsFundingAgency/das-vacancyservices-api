using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private VacancySearchParametersBuilder _builder;
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
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult());

            _mockStandardCodeConverter = new Mock<IStandardCodeConverter>();
            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult());

            _builder = new VacancySearchParametersBuilder(_mockStandardCodeConverter.Object, _mockFrameworkConverter.Object);
        }

        [Test]
        public async Task ThenTheStandardCodesAreReturned()
        {
            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult{SubCategoryCodes = _expectedStandards});

            var result = await _builder.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

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
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult
                {
                    SubCategoryCodes = new List<string> { "3498" },
                    ValidationFailures = validationFailures
                });

            var action = new Func<Task<VacancySearchParameters>>(() => _builder.ConvertFrom(new SearchApprenticeshipVacanciesRequest()));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {validationFailures[0].ErrorMessage}");
        }

        [Test]
        public async Task ThenTheFrameworkCodesAreReturned()
        {
            _mockFrameworkConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult { SubCategoryCodes = _expectedFrameworks });

            var result = await _builder.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(_expectedFrameworks);
        }

        [Test]
        public void AndFrameworkCodeValidationsAreReturned_ThenThrowsValidationException()
        {
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Framework", Guid.NewGuid().ToString())
            };

            _mockFrameworkConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult
                {
                    SubCategoryCodes = new List<string> { "3498" },
                    ValidationFailures = validationFailures
                });

            var action = new Func<Task<VacancySearchParameters>>(() => _builder.ConvertFrom(new SearchApprenticeshipVacanciesRequest()));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {validationFailures[0].ErrorMessage}");
        }

        [Test]
        public async Task AndFrameworkCodesAsWellAsStandardCodes_ThenBothAreReturned()
        {
            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult { SubCategoryCodes = _expectedStandards });

            _mockFrameworkConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult{SubCategoryCodes = _expectedFrameworks});

            var expectedSubCategoryCodes = new List<string>();
            expectedSubCategoryCodes.AddRange(_expectedStandards);
            expectedSubCategoryCodes.AddRange(_expectedFrameworks);

            var result = await _builder.ConvertFrom(new SearchApprenticeshipVacanciesRequest());

            result.SubCategoryCodes.ForEach(Console.WriteLine);

            result.SubCategoryCodes.ShouldAllBeEquivalentTo(expectedSubCategoryCodes);
        }

        [Test]
        public void AndFrameworkCodesAsWellAsStandardCodesValidationFailures_ThenBothAreInException()
        {
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("StandardCode", Guid.NewGuid().ToString()),
                new ValidationFailure("StandardCode", Guid.NewGuid().ToString()),
                new ValidationFailure("FrameworkCode", Guid.NewGuid().ToString()),
                new ValidationFailure("FrameworkCode", Guid.NewGuid().ToString())
            };

            var combinedErrorMessage = new StringBuilder();
            validationFailures.ForEach(failure => combinedErrorMessage.Append($"\r\n -- {failure.ErrorMessage}"));

            _mockStandardCodeConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult
                {
                    SubCategoryCodes = _expectedStandards,
                    ValidationFailures = validationFailures.Where(failure => failure.PropertyName == "StandardCode").ToList()
                });

            _mockFrameworkConverter
                .Setup(converter => converter.ConvertToSearchableCodesAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new SubCategoryConversionResult
                {
                    SubCategoryCodes = _expectedFrameworks,
                    ValidationFailures = validationFailures.Where(failure => failure.PropertyName == "FrameworkCode").ToList()
                });

            var action = new Func<Task<VacancySearchParameters>>(() => _builder.ConvertFrom(new SearchApprenticeshipVacanciesRequest()));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: {combinedErrorMessage}");
        }
    }
}