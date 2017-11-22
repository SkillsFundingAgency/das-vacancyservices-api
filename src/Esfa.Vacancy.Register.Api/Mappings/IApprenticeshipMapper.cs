using Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public interface IApprenticeshipMapper
    {
        ApprenticeshipVacancy MapToApprenticeshipVacancy(Domain.Entities.ApprenticeshipVacancy apprenticeshipVacancy);
    }
}