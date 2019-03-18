using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public interface IRecruitVacancyMapper
    {
        Task<ApprenticeshipVacancy> MapFromRecruitVacancy(SFA.DAS.Recruit.Vacancies.Client.Entities.Vacancy liveVacancy);
    }
}