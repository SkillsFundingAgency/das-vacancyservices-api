using System;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class HourlyWageCalculator : IHourlyWageCalculator
    {
        private const decimal WeeksPerMonth = 4m;
        private const decimal WeeksPerYear = 52m;
        private const string HoursPerWeekZeroErrorMessage = "HoursPerWeek must be greater than 0.";
        private const string IncorrectWageTypeErrorMessage = "WageUnit must be either 'Weekly', 'Monthly' or 'Annually'.";

        public decimal Calculate(decimal wageValue, WageUnit wageUnit, decimal hoursPerWeek)
        {
            if (hoursPerWeek <= 0)
                throw new ArgumentOutOfRangeException(nameof(hoursPerWeek), hoursPerWeek, HoursPerWeekZeroErrorMessage);

            decimal calculatedHourlyWage;

            switch (wageUnit)
            {
                case WageUnit.Weekly:
                    calculatedHourlyWage = decimal.Divide(wageValue, hoursPerWeek);
                    break;
                case WageUnit.Monthly:
                    calculatedHourlyWage = decimal.Divide(decimal.Divide(wageValue, WeeksPerMonth), hoursPerWeek);
                    break;
                case WageUnit.Annually:
                    calculatedHourlyWage = decimal.Divide(decimal.Divide(wageValue, WeeksPerYear), hoursPerWeek);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), wageUnit, IncorrectWageTypeErrorMessage);
            }

            return calculatedHourlyWage;
        }
    }
}