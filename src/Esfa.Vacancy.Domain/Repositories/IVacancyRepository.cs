using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Repositories
{
    public interface IVacancyRepository
    {
        Task<ApprenticeshipVacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber);
        Task<TraineeshipVacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber);
    }
}
