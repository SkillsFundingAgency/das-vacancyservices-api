using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Domain.Repositories
{
    public interface IStandardRepository
    {
        Task<IEnumerable<int>> GetStandardIdsAsync();
    }
}
