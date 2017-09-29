using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IStandardRepository
    {
        Task<IEnumerable<int>> GetDistinctStandardSectorIdsAsync(IEnumerable<int> standardIds);
    }
}
