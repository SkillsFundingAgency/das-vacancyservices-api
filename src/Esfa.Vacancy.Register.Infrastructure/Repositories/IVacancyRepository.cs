using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public interface IVacancyRepository
    {
        Task<Api.Types.Vacancy> GetVacancyByReferenceNumberAsync(int referenceNumber);
    }
}
