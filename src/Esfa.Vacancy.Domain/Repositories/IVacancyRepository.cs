using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Repositories
{
    public interface IVacancyRepository
    {
        Task<Entities.ApprenticeshipVacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber);
        Task<Entities.TraineeshipVacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber);
        Task<int> CreateApprenticeshipAsync(CreateApprenticeshipParameters parameters);
    }
}
