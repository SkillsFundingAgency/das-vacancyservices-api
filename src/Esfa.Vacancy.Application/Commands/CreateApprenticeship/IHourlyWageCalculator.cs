namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface IHourlyWageCalculator
    {
        decimal Calculate(decimal wageValue, WageUnit wageUnit, decimal hoursPerWeek);
    }
}