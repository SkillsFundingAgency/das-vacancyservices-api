using System;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using WageType = Esfa.Vacancy.Application.Commands.CreateApprenticeship.WageType;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAWageTypeMapper
{
    [TestFixture]
    public class WhenMappingWageType
    {
        [TestCase(WageType.Custom, LegacyWageType.Custom, TestName = "Custom")]
        [TestCase(WageType.NationalMinimumWage, LegacyWageType.NationalMinimum, TestName = "NationalMinimumWage")]
        [TestCase(WageType.ApprenticeshipMinimumWage, LegacyWageType.ApprenticeshipMinimum, TestName = "ApprenticeshipMinimumWage")]
        [TestCase(WageType.Unwaged, LegacyWageType.Unwaged, TestName = "Unwaged")]
        [TestCase(WageType.CompetitiveSalary, LegacyWageType.CompetitiveSalary, TestName = "CompetitiveSalary")]
        [TestCase(WageType.ToBeSpecified, LegacyWageType.ToBeAgreedUponAppointment, TestName = "ToBeSpecified")]
        public void ThenMapsToLegacyWageType(WageType originalWageType, LegacyWageType expectedLegacyWageType)
        {
            var wageTypeMapper = new WageTypeMapper();
            wageTypeMapper.MapToLegacy(originalWageType)
                .Should().Be(expectedLegacyWageType);
        }

        [Test]
        public void AndOutOfRange_ThenThrowsException()
        {
            var wageTypeMapper = new WageTypeMapper();
            Action action = () => wageTypeMapper.MapToLegacy((WageType)8435);
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}