using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

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
        public async Task CallCalculate(WageUnit wageUnit, decimal minWage, double hoursPerWeek, decimal expectedResult)
        {
            var minimumWageCalculator = new MinimumWageCalculator();

            var result =
                await minimumWageCalculator.CalculateMinimumWageAsync(
                    new CreateApprenticeshipRequest
                    {
                        WageUnit = wageUnit,
                        MinWage = minWage,
                        HoursPerWeek = hoursPerWeek
                    });

            result.Should().Be(expectedResult);
        }
    }

    public class MinimumWageCalculator
    {
        private const decimal WeeksPerMonth = 4m;
        private const decimal WeeksPerYear = 52m;

        public async Task<decimal> CalculateMinimumWageAsync(CreateApprenticeshipRequest request)
        {
            var actualHourlyWage = 0m;

            switch (request.WageUnit)
            {
                case WageUnit.Weekly:
                    actualHourlyWage = decimal.Divide(request.MinWage.Value, (decimal)request.HoursPerWeek);
                    break;
                case WageUnit.Monthly:
                    actualHourlyWage = decimal.Divide(decimal.Divide(request.MinWage.Value, WeeksPerMonth), (decimal)request.HoursPerWeek);
                    break;
                case WageUnit.Annually:
                    actualHourlyWage = decimal.Divide(decimal.Divide(request.MinWage.Value, WeeksPerYear), (decimal)request.HoursPerWeek);
                    break;
                default:
                    return decimal.MinValue;
                    /*throw new ArgumentOutOfRangeException(nameof(request.WageUnit), request.WageUnit, null);*/
            }
            return actualHourlyWage;
        }
    }
}