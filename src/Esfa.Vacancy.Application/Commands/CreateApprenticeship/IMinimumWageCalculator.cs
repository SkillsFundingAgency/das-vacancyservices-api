namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface IMinimumWageCalculator
    {
        decimal CalculateMinimumWage(CreateApprenticeshipRequest request);
    }
}