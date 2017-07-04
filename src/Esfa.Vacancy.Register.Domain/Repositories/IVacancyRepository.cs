using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IVacancyRepository
    {
        Task<Entities.Vacancy> GetVacancyByReferenceNumberAsync(int referenceNumber);
    }
}
