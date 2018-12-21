using System;
using SFA.DAS.VacancyServices.Wage;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class HourlyWageCalculator : IHourlyWageCalculator
    {
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
                    calculatedHourlyWage = WageCalculator.GetHourRate(wageValue, SFA.DAS.VacancyServices.Wage.WageUnit.Weekly, hoursPerWeek);
                    break;
                case WageUnit.Monthly:
                    calculatedHourlyWage = WageCalculator.GetHourRate(wageValue, SFA.DAS.VacancyServices.Wage.WageUnit.Monthly, hoursPerWeek);
                    break;
                case WageUnit.Annually:
                    calculatedHourlyWage = WageCalculator.GetHourRate(wageValue, SFA.DAS.VacancyServices.Wage.WageUnit.Annually, hoursPerWeek);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), wageUnit, IncorrectWageTypeErrorMessage);
            }

            return calculatedHourlyWage;
        }
    }
}