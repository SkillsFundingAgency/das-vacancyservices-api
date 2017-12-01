using Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.Manage.Api.Mappings
{
    public interface ICreateApprenticeshipResponseMapper
    {
        Vacancy.Api.Types.CreateApprecticeshipResponse MapToApiResponse(CreateApprenticeshipResponse apprenticeshipVacancy);
    }
}