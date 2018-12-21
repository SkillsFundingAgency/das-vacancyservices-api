using System;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAWageTextFormatter
{
    [TestFixture]
    public class WhenGettingWageText
    {
        [TestCase(WageType.CustomWageFixed, "£170.00", TestName = "Custom")]
        [TestCase(WageType.CustomWageRange, "£150.00 - £290.00", TestName = "CustomRange")]
        [TestCase(WageType.NationalMinimumWage, "£147.00 - £274.05", TestName = "NationalMinimumWage")]
        [TestCase(WageType.ApprenticeshipMinimumWage, "£129.50", TestName = "ApprenticeshipMinimumWage")]
        [TestCase(WageType.Unwaged, "Unwaged", TestName = "Unwaged")]
        [TestCase(WageType.CompetitiveSalary, "Competitive salary", TestName = "CompetitiveSalary")]
        [TestCase(WageType.ToBeSpecified, "To be agreed upon appointment", TestName = "ToBeSpecified")]
        public void ThenGetsWageText(WageType wageType, string wageText)
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = wageType,
                HoursPerWeek = 35,
                ExpectedStartDate = new DateTime(2018, 4, 1),
                FixedWage = 170,
                MinWage = 150,
                MaxWage = 290
            };
            var wageTextFormatter = new WageTextFormatter();

            wageTextFormatter.GetWageText(request).Should().Be(wageText);
        }
    }
}
