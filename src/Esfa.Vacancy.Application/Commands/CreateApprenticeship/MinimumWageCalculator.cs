using System;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class MinimumWageCalculator : IMinimumWageCalculator
    {
        private const decimal WeeksPerMonth = 4m;
        private const decimal WeeksPerYear = 52m;
        private const string MissingMinWageErrorMessage = "MinWage can't be null.";
        private const string IncorrectWageTypeErrorMessage = "WageUnit must be either 'Weekly', 'Monthly' or 'Annually'.";

        public decimal CalculateMinimumWage(CreateApprenticeshipRequest request)
        {
            if (!request.MinWage.HasValue)
                throw new ArgumentOutOfRangeException(nameof(request.MinWage), request.MinWage, MissingMinWageErrorMessage);

            decimal calculatedMinWage;

            switch (request.WageUnit)
            {
                case WageUnit.Weekly:
                    calculatedMinWage = decimal.Divide(request.MinWage.Value, (decimal)request.HoursPerWeek);
                    break;
                case WageUnit.Monthly:
                    calculatedMinWage = decimal.Divide(decimal.Divide(request.MinWage.Value, WeeksPerMonth), (decimal)request.HoursPerWeek);
                    break;
                case WageUnit.Annually:
                    calculatedMinWage = decimal.Divide(decimal.Divide(request.MinWage.Value, WeeksPerYear), (decimal)request.HoursPerWeek);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.WageUnit), request.WageUnit, IncorrectWageTypeErrorMessage);
            }

            return decimal.Round(calculatedMinWage,2);
        }
    }
}