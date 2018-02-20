using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAMinimumWageCalculator
{
    [TestFixture]
    public class WhenCallingCalculate
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(WageUnit.Weekly, 120m, 40, 3m).SetName("Then calculates correct value from weekly"),
            new TestCaseData(WageUnit.Weekly, 130.0m, 36.0, 3.61m).SetName("Then calculates correct value from weekly"),
            new TestCaseData(WageUnit.Weekly, 150m, 30, 5m).SetName("Then calculates correct value from weekly"),
            new TestCaseData(WageUnit.Monthly, 480m, 40, 3m).SetName("Then calculates correct value from monthly"),
            new TestCaseData(WageUnit.Monthly, 525m, 37.5, 3.5m).SetName("Then calculates correct value from monthly"),
            new TestCaseData(WageUnit.Monthly, 600m, 30, 5m).SetName("Then calculates correct value from monthly"),
            new TestCaseData(WageUnit.Annually, 6240m, 40, 3m).SetName("Then calculates correct value from annually"),
            new TestCaseData(WageUnit.Annually, 6825m, 37.5, 3.5m).SetName("Then calculates correct value from annually"),
            new TestCaseData(WageUnit.Annually, 7800m, 30, 5m).SetName("Then calculates correct value from annually")
        };

        [TestCaseSource(nameof(TestCases))]
        public void RunTestCases(WageUnit wageUnit, decimal minWage, double hoursPerWeek, decimal expectedResult)
        {
            var minimumWageCalculator = new MinimumWageCalculator();

            var result = minimumWageCalculator.CalculateMinimumWage(minWage, wageUnit, (decimal)hoursPerWeek);

            result.Should().BeApproximately(expectedResult, 2);
        }

        [Test]
        public void AndNoHoursPerWeek_ThenThrowsException()
        {
            var missingMinWageErrorMessage = "HoursPerWeek must be greater than 0.";
            var minimumWageCalculator = new MinimumWageCalculator();
            Action action = () => minimumWageCalculator.CalculateMinimumWage(34m, WageUnit.Weekly, 0m);

            action.ShouldThrow<ArgumentOutOfRangeException>()
                  .WithMessage($"{missingMinWageErrorMessage}\r\nParameter name: HoursPerWeek\r\nActual value was 0.");
        }

        [Test]
        public void AndNotExpectedWageUnit_ThenThrowsException()
        {
            var expectedMessage = "WageUnit must be either 'Weekly', 'Monthly' or 'Annually'.";
            var minimumWageCalculator = new MinimumWageCalculator();

            Action action = () => minimumWageCalculator.CalculateMinimumWage(300m, 0, 234m);

            action.ShouldThrow<ArgumentOutOfRangeException>()
                  .WithMessage($"{expectedMessage}\r\nParameter name: WageUnit\r\nActual value was 0.");
        }
    }
}
