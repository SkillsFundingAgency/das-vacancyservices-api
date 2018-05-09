using Esfa.Vacancy.Api.Types;
using SFA.DAS.Recruit.Vacancies.Client.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public interface IRecruitVacancyMapper
    {
        ApprenticeshipVacancy MapFromRecruitVacancy(LiveVacancy liveVacancy);
    }
}