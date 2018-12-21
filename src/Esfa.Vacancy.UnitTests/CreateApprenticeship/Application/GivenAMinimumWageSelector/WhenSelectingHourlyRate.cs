using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SFA.DAS.VacancyServices.Wage;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAMinimumWageSelector
{
    [TestFixture]
    public class WhenSelectingHourlyRate
    {
        private static readonly WageRange FirstWageRange = new WageRange
        {
            ApprenticeMinimumWage = 3.5m,
            ValidFrom = DateTime.MinValue,
            ValidTo = DateTime.Today.AddMonths(-2)
        };

        private static readonly WageRange SecondWageRange = new WageRange
        {
            ApprenticeMinimumWage = 3.7m,
            ValidFrom = DateTime.Today.AddMonths(-2).AddDays(1).AddHours(12),
            ValidTo = DateTime.Today.AddMonths(2).AddHours(12)
        };

        private static readonly WageRange ThirdWageRange = new WageRange
        {
            ApprenticeMinimumWage = 3.7m,
            ValidFrom = DateTime.Today.AddMonths(2).AddDays(1),
            ValidTo = DateTime.MaxValue
        };

        private Mock<IGetMinimumWagesService> _mockMinimumWageService;
        private IGetMinimumWagesService _getMinimumWagesService;
        private IFixture _fixture;

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(DateTime.Today.AddMonths(-15), FirstWageRange.ApprenticeMinimumWage)
                .SetName("And matches oldest rate Then returns oldest rate"),
            new TestCaseData(DateTime.Today, SecondWageRange.ApprenticeMinimumWage)
                .SetName("And matches middle rate Then returns middle rate"),
            new TestCaseData(DateTime.Today.AddMonths(3), ThirdWageRange.ApprenticeMinimumWage)
                .SetName("And matches newest rate Then returns newest rate"),
            new TestCaseData(DateTime.Today.AddMonths(2).AddHours(2), SecondWageRange.ApprenticeMinimumWage)
                .SetName("And expectedStartDate has time Then strips time to get correct wage range"),
            new TestCaseData(DateTime.Today.AddMonths(-2).AddDays(1), SecondWageRange.ApprenticeMinimumWage)
                .SetName("And range.ValidFrom has time Then strips time to get correct wage range"),
            new TestCaseData(DateTime.Today.AddMonths(2), SecondWageRange.ApprenticeMinimumWage)
                .SetName("And range.ValidTo has time Then strips time to get correct wage range")
        };

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _mockMinimumWageService = _fixture.Freeze<Mock<IGetMinimumWagesService>>();
            _mockMinimumWageService
                .Setup(service => service.GetWageRange(It.IsAny<DateTime>()))
                .Returns<DateTime>(NationalMinimumWageService.GetHourlyRates);

            _getMinimumWagesService = _fixture.Create<GetMinimumWagesService>();
        }

        [TestCaseSource(nameof(TestCases))]
        public void WhenCallingSelectHourlyRate(DateTime expectedStartDate, decimal expectedHourlyRate)
        {
            var hourlyRate = _getMinimumWagesService.GetApprenticeMinimumWageRate(expectedStartDate);

            hourlyRate.Should().Be(expectedHourlyRate);
        }
    }
}