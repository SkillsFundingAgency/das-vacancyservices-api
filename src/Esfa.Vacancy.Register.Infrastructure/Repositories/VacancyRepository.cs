using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        public async Task<Api.Types.Vacancy> GetVacancyByReferenceNumberAsync(int referenceNumber)
        {
            await Task.Delay(10);
            return null;
        }
    }
}