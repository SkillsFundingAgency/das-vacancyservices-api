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
            new TestCaseData(WageUnit.Weekly, 131.25m, 37.5, 3.5m).SetName("Then calculates correct value from weekly"),
            new TestCaseData(WageUnit.Weekly, 150m, 30, 5m).SetName("Then calculates correct value from weekly"),
            new TestCaseData(WageUnit.Weekly, 151m, 30, 5.03m).SetName("Then rounds for weekly"),
            new TestCaseData(WageUnit.Monthly, 480m, 40, 3m).SetName("Then calculates correct value from monthly"),
            new TestCaseData(WageUnit.Monthly, 525m, 37.5, 3.5m).SetName("Then calculates correct value from monthly"),
            new TestCaseData(WageUnit.Monthly, 600m, 30, 5m).SetName("Then calculates correct value from monthly"),
            new TestCaseData(WageUnit.Monthly, 601m, 30, 5.01m).SetName("Then rounds for monthly"),
            new TestCaseData(WageUnit.Annually, 6240m, 40, 3m).SetName("Then calculates correct value from annually"),
            new TestCaseData(WageUnit.Annually, 6825m, 37.5, 3.5m).SetName("Then calculates correct value from annually"),
            new TestCaseData(WageUnit.Annually, 7800m, 30, 5m).SetName("Then calculates correct value from annually"),
            new TestCaseData(WageUnit.Annually, 7001m, 30, 4.49m).SetName("Then rounds for annually")
        };

        [TestCaseSource(nameof(TestCases))]
        public void RunTestCases(WageUnit wageUnit, decimal minWage, double hoursPerWeek, decimal expectedResult)
        {
            var minimumWageCalculator = new MinimumWageCalculator();

            var result =
                minimumWageCalculator.CalculateMinimumWage(
                    new CreateApprenticeshipRequest
                    {
                        WageUnit = wageUnit,
                        MinWage = minWage,
                        HoursPerWeek = hoursPerWeek
                    });

            result.Should().Be(expectedResult);
        }

        [Test]
        public void AndNoMinWage_ThenThrowsException()
        {
            var missingMinWageErrorMessage = "MinWage can't be null.";
            var minimumWageCalculator = new MinimumWageCalculator();
            Action action = () => minimumWageCalculator.CalculateMinimumWage(
                new CreateApprenticeshipRequest
                {
                    WageUnit = WageUnit.Weekly,
                    HoursPerWeek = 20
                }); 
            action.ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessage($"{missingMinWageErrorMessage}\r\nParameter name: MinWage");
        }

        [Test]
        public void AndNoHoursPerWeek_ThenThrowsException()
        {
            var missingMinWageErrorMessage = "HoursPerWeek must be greater than 0.";
            var minimumWageCalculator = new MinimumWageCalculator();
            Action action = () => minimumWageCalculator.CalculateMinimumWage(
                new CreateApprenticeshipRequest
                {
                    MinWage = 34m,
                    WageUnit = WageUnit.Weekly
                });
            action.ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessage($"{missingMinWageErrorMessage}\r\nParameter name: HoursPerWeek\r\nActual value was 0.");
        }

        [Test]
        public void AndNotExpectedWageUnit_ThenThrowsException()
        {
            var expectedMessage = "WageUnit must be either 'Weekly', 'Monthly' or 'Annually'.";
            var minimumWageCalculator = new MinimumWageCalculator();

            Action action = () => minimumWageCalculator.CalculateMinimumWage(
                new CreateApprenticeshipRequest
                {
                    MinWage = 300m,
                    HoursPerWeek = 234,
                    WageType = WageType.Custom
                });

            action.ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessage($"{expectedMessage}\r\nParameter name: WageUnit\r\nActual value was 0.");
        }

        [Test]
        public void ThenRoundsToNearestEvenNumber()
        {
            var minimumWageCalculator = new MinimumWageCalculator();

            var result =
                minimumWageCalculator.CalculateMinimumWage(
                    new CreateApprenticeshipRequest
                    {
                        WageUnit = WageUnit.Annually,
                        MinWage = 5766.8m,
                        HoursPerWeek = 20
                    });

            result.Should().Be(5.54m);
        }
    }
}