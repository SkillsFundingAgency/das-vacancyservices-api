namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface IWageTextFormatter
    {
        string GetWageText(CreateApprenticeshipRequest request);
    }
}