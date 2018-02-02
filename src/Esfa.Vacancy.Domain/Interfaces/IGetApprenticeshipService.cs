using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Interfaces
{
    public interface IGetApprenticeshipService
    {
        Task<ApprenticeshipVacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber);
    }
}