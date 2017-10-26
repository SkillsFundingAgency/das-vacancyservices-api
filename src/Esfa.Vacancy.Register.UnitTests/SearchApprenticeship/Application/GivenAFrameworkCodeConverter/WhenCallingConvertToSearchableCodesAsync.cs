using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAFrameworkCodeConverter
{
    [TestFixture]
    public class WhenCallingConvertToSearchableCodesAsync
    {
        private FrameworkCodeConverter _frameworkCodeConverter;
        private List<string> _frameworks;
        private Mock<IFrameworkCodeRepository> _mockFrameworkRepository;

        [SetUp]
        public void Setup()
        {
            _frameworks = new List<string> {"3454", "3876", "6854"};

            _mockFrameworkRepository = new Mock<IFrameworkCodeRepository>();
            _mockFrameworkRepository
                .Setup(repository => repository.GetAsync())
                .ReturnsAsync(_frameworks);

            _frameworkCodeConverter = new FrameworkCodeConverter(_mockFrameworkRepository.Object);
        }

        [Test]
        public async Task AndNoFrameworkIsFoundForOneToConvert_ThenThrowsValidationException()
        {
            var result = await _frameworkCodeConverter.ConvertToSearchableCodesAsync(new List<string> { "99999" });

            result.ValidationFailures.ShouldBeEquivalentTo(new List<ValidationFailure>
            {
                new ValidationFailure("FrameworkCode", string.Format(ErrorMessages.SearchApprenticeships.FrameworkCodeNotFound, "99999"))
                { ErrorCode = ErrorCodes.SearchApprenticeships.FrameworkCodeNotFound }
            });
        }

        [Test]
        public async Task AndNoFrameworkIsFoundForSeveralToConvert_ThenExceptionIncludesAllValidationFailures()
        {
            var result = await _frameworkCodeConverter.ConvertToSearchableCodesAsync(new List<string> { "77777", "88888" });

            result.ValidationFailures.ShouldBeEquivalentTo(new List<ValidationFailure>
            {
                new ValidationFailure("FrameworkCode", string.Format(ErrorMessages.SearchApprenticeships.FrameworkCodeNotFound, "77777"))
                { ErrorCode = ErrorCodes.SearchApprenticeships.FrameworkCodeNotFound },
                new ValidationFailure("FrameworkCode", string.Format(ErrorMessages.SearchApprenticeships.FrameworkCodeNotFound, "88888"))
                { ErrorCode = ErrorCodes.SearchApprenticeships.FrameworkCodeNotFound }
            });
        }

        [Test]
        public async Task AndFrameworkCodeIsFound_ThenReturnsFramework()
        {
            var result = await _frameworkCodeConverter.ConvertToSearchableCodesAsync(new List<string> {_frameworks[0]});

            result.SubCategoryCodes.Should().BeEquivalentTo(new List<string> {$"FW.{_frameworks[0]}"});
        }

        [Test]
        public async Task AndFrameworkCodeHasSpace_ThenReturnsCorrectFormat()
        {
            var result = await _frameworkCodeConverter.ConvertToSearchableCodesAsync(new List<string> { $" {_frameworks[1]} " });

            result.SubCategoryCodes.Should().BeEquivalentTo(new List<string> { $"FW.{_frameworks[1]}" });
        }

        [Test]
        public async Task AndNoFrameworks_ThenReturnsEmptyList()
        {
            var result = await _frameworkCodeConverter.ConvertToSearchableCodesAsync(new List<string>());

            result.SubCategoryCodes.Should().BeEmpty();
        }

        [Test]
        public async Task AndNoFrameworks_ThenDoesNotCallRepository()
        {
            await _frameworkCodeConverter.ConvertToSearchableCodesAsync(new List<string>());
            _mockFrameworkRepository.Verify(repository => repository.GetAsync(), Times.Never);
        }
    }
}