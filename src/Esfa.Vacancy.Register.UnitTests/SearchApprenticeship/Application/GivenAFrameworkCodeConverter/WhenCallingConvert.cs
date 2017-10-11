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

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAFrameworkCodeConverter
{
    [TestFixture]
    public class WhenCallingConvert
    {
        private FrameworkCodeConverter _frameworkCodeConverter;
        private List<Framework> _frameworks;

        [SetUp]
        public void Setup()
        {
            _frameworks = new List<Framework>
            {
                new Framework {Code = 3454, Title = Guid.NewGuid().ToString(), Uri = Guid.NewGuid().ToString()},
                new Framework {Code = 3876, Title = Guid.NewGuid().ToString(), Uri = Guid.NewGuid().ToString()},
                new Framework {Code = 6854, Title = Guid.NewGuid().ToString(), Uri = Guid.NewGuid().ToString()}
            };

            var mockFrameworkRepository = new Mock<IFrameworkRepository>();
            mockFrameworkRepository
                .Setup(repository => repository.GetFrameworksAsync())
                .ReturnsAsync(_frameworks);

            _frameworkCodeConverter = new FrameworkCodeConverter(mockFrameworkRepository.Object);
        }

        [Test]
        public void AndNoFrameworkIsFoundForOneToConvert_ThenThrowsValidationException()
        {
            var action = new Func<Task<List<string>>>(() => _frameworkCodeConverter.Convert(new List<string> { "99999" }));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- FrameworkCode 99999 is invalid");
        }

        [Test]
        public void AndNoFrameworkIsFoundForSeveralToConvert_ThenExceptionIncludesAllValidationFailures()
        {
            var action = new Func<Task<List<string>>>(() => _frameworkCodeConverter.Convert(new List<string> { "77777", "88888" }));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- FrameworkCode 77777 is invalid\r\n -- FrameworkCode 88888 is invalid");
        }

        [Test]
        public async Task AndFrameworkCodeIsFound_ThenReturnsFramework()
        {
            var result = await _frameworkCodeConverter.Convert(new List<string> {_frameworks[0].Code.ToString()});

            result.Should().BeEquivalentTo(new List<string> {$"FW.{_frameworks[0].Code}"});
        }

        [Test]
        public async Task AndFrameworkCodeHasSpace_ThenReturnsCorrectFormat()
        {
            var result = await _frameworkCodeConverter.Convert(new List<string> { $" {_frameworks[1].Code} " });

            result.Should().BeEquivalentTo(new List<string> { $"FW.{_frameworks[1].Code}" });
        }

        [Test]
        public async Task AndNoFrameworks_ThenReturnsEmptyList()
        {
            var result = await _frameworkCodeConverter.Convert(new List<string>());

            result.Should().BeEmpty();
        }
    }
}