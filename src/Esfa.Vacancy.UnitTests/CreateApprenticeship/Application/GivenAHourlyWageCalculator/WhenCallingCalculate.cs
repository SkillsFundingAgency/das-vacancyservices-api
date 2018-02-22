using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAHourlyWageCalculator
{
    [TestFixture]
    public class WhenCallingCalculate
    {
        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(WageUnit.Weekly, 120m, 40, 3m).SetName("Then calculates exact value from weekly"),
            new TestCaseData(WageUnit.Weekly, 130.0m, 36.0, 3.61m).SetName("Then calculates approximate value from weekly"),
            new TestCaseData(WageUnit.Monthly, 481m, 40, 2.77m).SetName("Then calculates approximate value from monthly"),
            new TestCaseData(WageUnit.Monthly, 568.75m, 37.5, 3.5m).SetName("Then calculates exact value from monthly"),
            new TestCaseData(WageUnit.Annually, 6825m, 37.5, 3.5m).SetName("Then calculates exact value from annually"),
            new TestCaseData(WageUnit.Annually, 6826m, 37.5, 3.5m).SetName("Then calculates approximate value from annually")
        };

        [TestCaseSource(nameof(TestCases))]
        public void RunTestCases(WageUnit wageUnit, decimal minWage, double hoursPerWeek, decimal expectedResult)
        {
            var minimumWageCalculator = new HourlyWageCalculator();

            var result = minimumWageCalculator.Calculate(minWage, wageUnit, (decimal)hoursPerWeek);

            result.Should().BeApproximately(expectedResult, 0.005m);
        }

        [Test]
        public void AndNoHoursPerWeek_ThenThrowsException()
        {
            var missingMinWageErrorMessage = "HoursPerWeek must be greater than 0.";
            var minimumWageCalculator = new HourlyWageCalculator();
            Action action = () => minimumWageCalculator.Calculate(34m, WageUnit.Weekly, 0m);

            action.ShouldThrow<ArgumentOutOfRangeException>()
                  .WithMessage($"{missingMinWageErrorMessage}\r\nParameter name: HoursPerWeek\r\nActual value was 0.");
        }

        [Test]
        public void AndNotExpectedWageUnit_ThenThrowsException()
        {
            var expectedMessage = "WageUnit must be either 'Weekly', 'Monthly' or 'Annually'.";
            var minimumWageCalculator = new HourlyWageCalculator();

            Action action = () => minimumWageCalculator.Calculate(300m, 0, 234m);

            action.ShouldThrow<ArgumentOutOfRangeException>()
                  .WithMessage($"{expectedMessage}\r\nParameter name: WageUnit\r\nActual value was 0.");
        }
    }
}
