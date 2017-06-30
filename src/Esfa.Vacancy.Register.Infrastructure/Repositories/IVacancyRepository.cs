using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public interface IVacancyRepository
    {
        Task<Domain.Entities.Vacancy> GetVacancyByReferenceNumberAsync(int referenceNumber);
    }
}
