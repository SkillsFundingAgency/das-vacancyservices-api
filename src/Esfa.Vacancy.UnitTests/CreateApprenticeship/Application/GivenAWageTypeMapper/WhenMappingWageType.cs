using System;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAWageTypeMapper
{
    [TestFixture]
    public class WhenMappingWageType
    {
        [TestCase(WageType.Custom, 345, null, LegacyWageType.Custom, TestName = "Custom")]
        [TestCase(WageType.Custom, 345, 345, LegacyWageType.Custom, TestName = "Custom")]
        [TestCase(WageType.Custom, 345, 346, LegacyWageType.CustomRange, TestName = "CustomRange")]
        [TestCase(WageType.NationalMinimumWage, null, null, LegacyWageType.NationalMinimum, TestName = "NationalMinimumWage")]
        [TestCase(WageType.ApprenticeshipMinimumWage, null, null, LegacyWageType.ApprenticeshipMinimum, TestName = "ApprenticeshipMinimumWage")]
        [TestCase(WageType.Unwaged, null, null, LegacyWageType.Unwaged, TestName = "Unwaged")]
        [TestCase(WageType.CompetitiveSalary, null, null, LegacyWageType.CompetitiveSalary, TestName = "CompetitiveSalary")]
        [TestCase(WageType.ToBeSpecified, null, null, LegacyWageType.ToBeAgreedUponAppointment, TestName = "ToBeSpecified")]
        public void ThenMapsToLegacyWageType(WageType originalWageType, decimal? minWage, decimal? maxWage, LegacyWageType expectedLegacyWageType)
        {
            var wageTypeMapper = new WageTypeMapper();
            var request = new CreateApprenticeshipRequest
            {
                WageType = originalWageType,
                MinWage = minWage,
                MaxWage = maxWage
            };
            wageTypeMapper.MapToLegacy(request)
                .Should().Be(expectedLegacyWageType);
        }

        [Test]
        public void AndOutOfRange_ThenThrowsException()
        {
            var wageTypeMapper = new WageTypeMapper();
            var request = new CreateApprenticeshipRequest { WageType = (WageType)8435 };
            Action action = () => wageTypeMapper.MapToLegacy(request);
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}