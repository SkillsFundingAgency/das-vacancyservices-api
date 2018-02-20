namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface IMinimumWageCalculator
    {
        decimal CalculateMinimumWage(decimal wageValue, WageUnit wageUnit, decimal hoursPerWeek);
    }
}