using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IVacancyRepository
    {
        Task<Entities.ApprenticeshipVacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber);
        Task<Entities.TraineeshipVacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber);
    }
}
