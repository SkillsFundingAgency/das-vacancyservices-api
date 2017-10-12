using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAStandardCodeConverter
{
    [TestFixture]
    public class WhenCallingConvert
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
        public void AndNoStandardSectorIsFoundForOneToConvert_ThenThrowsValidationException()
        {
            var action = new Func<Task<List<string>>>(() => _standardCodeConverter.Convert(new List<string> { "99999" }));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- StandardCode 99999 is invalid");
        }

        [Test]
        public void AndNoStandardSectorIsFoundForSeveralToConvert_ThenExceptionIncludesAllValidationFailures()
        {
            var action = new Func<Task<List<string>>>(() => _standardCodeConverter.Convert(new List<string> { "77777", "88888" }));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- StandardCode 77777 is invalid\r\n -- StandardCode 88888 is invalid");
        }

        [Test]
        public async Task AndStandardSectorIsFound_ThenReturnsStandard()
        {
            var result = await _standardCodeConverter.Convert(new List<string> { _standardSectors[0].LarsCode.ToString() });

            result.Should().BeEquivalentTo(new List<string> { $"{StandardSector.StandardSectorPrefix}.{_standardSectors[0].StandardSectorId}" });
        }

        [Test]
        public async Task AndStandardCodeHasSpace_ThenReturnsCorrectFormat()
        {
            var result = await _standardCodeConverter.Convert(new List<string> { $" {_standardSectors[1].LarsCode} " });

            result.Should().BeEquivalentTo(new List<string> { $"{StandardSector.StandardSectorPrefix}.{_standardSectors[1].StandardSectorId}" });
        }

        [Test]
        public async Task AndNoStandards_ThenReturnsEmptyList()
        {
            var result = await _standardCodeConverter.Convert(new List<string>());

            result.Should().BeEmpty();
        }

        [Test]
        public async Task AndNoStandards_ThenDoesNotCallRepository()
        {
            await _standardCodeConverter.Convert(new List<string>());
            _mockStandardRepository.Verify(repository => repository.GetStandardsAndRespectiveSectorIdsAsync(), Times.Never);
        }
    }
}