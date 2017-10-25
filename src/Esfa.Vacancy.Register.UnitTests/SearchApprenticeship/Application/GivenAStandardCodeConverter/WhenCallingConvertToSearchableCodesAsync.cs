using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAStandardCodeConverter
{
    [TestFixture]
    public class WhenCallingConvertToSearchableCodesAsync
    {
        private StandardCodeConverter _standardCodeConverter;
        private List<StandardSector> _standardSectors;
        private Mock<IStandardRepository> _mockStandardRepository;

        [SetUp]
        public void Setup()
        {
            _standardSectors = new List<StandardSector>
            {
                new StandardSector() {LarsCode = 100, StandardSectorId = 1},
                new StandardSector() {LarsCode = 110, StandardSectorId = 1},
                new StandardSector() {LarsCode = 200, StandardSectorId = 2},
                new StandardSector() {LarsCode = 210, StandardSectorId = 2},
                new StandardSector() {LarsCode = 300, StandardSectorId = 3},
                new StandardSector() {LarsCode = 310, StandardSectorId = 3}
            };

            _mockStandardRepository = new Mock<IStandardRepository>();
            _mockStandardRepository
                .Setup(repository => repository.GetStandardsAndRespectiveSectorIdsAsync())
                .ReturnsAsync(_standardSectors);

            _standardCodeConverter = new StandardCodeConverter(_mockStandardRepository.Object);
        }

        [Test]
        public async Task AndNoStandardSectorIsFoundForOneToConvert_ThenThrowsValidationException()
        {
            var result = await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string> { "99999" });

            result.ValidationFailures.ShouldBeEquivalentTo(new List<ValidationFailure>
            {
                new ValidationFailure("StandardCode", "StandardCode 99999 is invalid")
                { ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotFound }
            });
        }

        [Test]
        public async Task AndNoStandardSectorIsFoundForSeveralToConvert_ThenExceptionIncludesAllValidationFailures()
        {
            var result = await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string> { "77777", "88888" });

            result.ValidationFailures.ShouldBeEquivalentTo(new List<ValidationFailure>
            {
                new ValidationFailure("StandardCode", "StandardCode 77777 is invalid")
                { ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotFound },
                new ValidationFailure("StandardCode", "StandardCode 88888 is invalid")
                { ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotFound }
            });
        }

        [Test]
        public async Task AndSpaceInSectorCode_ThenExceptionMessageHasNoExtraSpace()
        {
            var result = await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string> { " 99999" });

            result.ValidationFailures.ShouldBeEquivalentTo(new List<ValidationFailure>
            {
                new ValidationFailure("StandardCode", "StandardCode 99999 is invalid")
                    { ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotFound }
            });
        }

        [Test]
        public async Task AndStandardSectorIsFound_ThenReturnsStandard()
        {
            var result = await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string> { _standardSectors[0].LarsCode.ToString() });

            result.SubCategoryCodes.Should().BeEquivalentTo(new List<string> { $"{StandardSector.StandardSectorPrefix}.{_standardSectors[0].StandardSectorId}" });
        }

        [Test]
        public async Task AndStandardCodeHasSpace_ThenReturnsCorrectFormat()
        {
            var result = await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string> { $" {_standardSectors[1].LarsCode} " });

            result.SubCategoryCodes.Should().BeEquivalentTo(new List<string> { $"{StandardSector.StandardSectorPrefix}.{_standardSectors[1].StandardSectorId}" });
        }

        [Test]
        public async Task AndNoStandards_ThenReturnsEmptyList()
        {
            var result = await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string>());

            result.SubCategoryCodes.Should().BeEmpty();
        }

        [Test]
        public async Task AndNoStandards_ThenDoesNotCallRepository()
        {
            await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string>());
            _mockStandardRepository.Verify(repository => repository.GetStandardsAndRespectiveSectorIdsAsync(), Times.Never);
        }

        [Test]
        public async Task ThenConvertToDistinctSectorCodes()
        {
            var response = await _standardCodeConverter.ConvertToSearchableCodesAsync(new List<string>() { "100", "110", "200", "210" });

            response.SubCategoryCodes.Count.Should().Be(2);
            response.SubCategoryCodes.Should().Contain($"{StandardSector.StandardSectorPrefix}.{1}");
            response.SubCategoryCodes.Should().Contain($"{StandardSector.StandardSectorPrefix}.{2}");
        }
    }
}