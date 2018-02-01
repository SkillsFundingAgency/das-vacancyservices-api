using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface ICreateApprenticeshipParametersMapper
    {
        CreateApprenticeshipParameters MapFromRequest(CreateApprenticeshipRequest request,
            EmployerInformation employerInformation);
    }
}