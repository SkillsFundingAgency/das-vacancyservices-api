using System;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class MinimumWageCalculator : IMinimumWageCalculator
    {
        private const decimal WeeksPerMonth = 4m;
        private const decimal WeeksPerYear = 52m;
        private const string HoursPerWeekZeroErrorMessage = "HoursPerWeek must be greater than 0.";
        private const string IncorrectWageTypeErrorMessage = "WageUnit must be either 'Weekly', 'Monthly' or 'Annually'.";

        public decimal CalculateMinimumWage(decimal wageValue, WageUnit wageUnit, decimal hoursPerWeek)
        {
            if (!(hoursPerWeek > 0))
                throw new ArgumentOutOfRangeException(nameof(hoursPerWeek), hoursPerWeek, HoursPerWeekZeroErrorMessage);

            decimal calculatedMinWage;

            switch (wageUnit)
            {
                case WageUnit.Weekly:
                    calculatedMinWage = decimal.Divide(wageValue, hoursPerWeek);
                    break;
                case WageUnit.Monthly:
                    calculatedMinWage = decimal.Divide(decimal.Divide(wageValue, WeeksPerMonth), hoursPerWeek);
                    break;
                case WageUnit.Annually:
                    calculatedMinWage = decimal.Divide(decimal.Divide(wageValue, WeeksPerYear), hoursPerWeek);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), wageUnit, IncorrectWageTypeErrorMessage);
            }

            return calculatedMinWage;
        }
    }
}