using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Interfaces
{
    public interface IGetTraineeshipService
    {
        Task<TraineeshipVacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber);
    }
}