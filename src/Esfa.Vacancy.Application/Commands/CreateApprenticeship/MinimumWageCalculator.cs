using System;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
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