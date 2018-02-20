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
        [TestCase(WageType.CustomWageFixed, LegacyWageType.Custom, TestName = "Custom")]
        [TestCase(WageType.CustomWageRange, LegacyWageType.CustomRange, TestName = "CustomRange")]
        [TestCase(WageType.NationalMinimumWage, LegacyWageType.NationalMinimum, TestName = "NationalMinimumWage")]
        [TestCase(WageType.ApprenticeshipMinimumWage, LegacyWageType.ApprenticeshipMinimum, TestName = "ApprenticeshipMinimumWage")]
        [TestCase(WageType.Unwaged, LegacyWageType.Unwaged, TestName = "Unwaged")]
        [TestCase(WageType.CompetitiveSalary, LegacyWageType.CompetitiveSalary, TestName = "CompetitiveSalary")]
        [TestCase(WageType.ToBeSpecified, LegacyWageType.ToBeAgreedUponAppointment, TestName = "ToBeSpecified")]
        public void ThenMapsToLegacyWageType(WageType originalWageType, LegacyWageType expectedLegacyWageType)
        {
            var wageTypeMapper = new WageTypeMapper();
            var request = new CreateApprenticeshipRequest
            {
                WageType = originalWageType,
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