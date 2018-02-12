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
                    WageUnit = WageUnit.Weekly
                }); 
            action.ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessage($"{missingMinWageErrorMessage}\r\nParameter name: MinWage");
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
                    WageType = WageType.Custom
                });

            action.ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessage($"{expectedMessage}\r\nParameter name: WageUnit\r\nActual value was 0.");
        }
    }

    public class MinimumWageCalculator
    {
        private const decimal WeeksPerMonth = 4m;
        private const decimal WeeksPerYear = 52m;
        private const string MissingMinWageErrorMessage = "MinWage can't be null.";
        private const string IncorrectWageTypeErrorMessage = "WageUnit must be either 'Weekly', 'Monthly' or 'Annually'.";

        public decimal CalculateMinimumWage(CreateApprenticeshipRequest request)
        {
            if (!request.MinWage.HasValue)
                throw new ArgumentOutOfRangeException(nameof(request.MinWage), request.MinWage, MissingMinWageErrorMessage);

            switch (request.WageUnit)
            {
                case WageUnit.Weekly:
                    return decimal.Divide(request.MinWage.Value, (decimal)request.HoursPerWeek);
                case WageUnit.Monthly:
                    return decimal.Divide(decimal.Divide(request.MinWage.Value, WeeksPerMonth), (decimal)request.HoursPerWeek);
                case WageUnit.Annually:
                    return decimal.Divide(decimal.Divide(request.MinWage.Value, WeeksPerYear), (decimal)request.HoursPerWeek);
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.WageUnit), request.WageUnit, IncorrectWageTypeErrorMessage);
            }
        }
    }
}