using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IVacancyRepository
    {
        Task<Entities.Vacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber);
        Task<Entities.Vacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber);
    }
}
