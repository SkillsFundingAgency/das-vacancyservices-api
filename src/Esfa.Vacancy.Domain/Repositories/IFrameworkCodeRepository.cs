using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Domain.Repositories
{
    public interface IFrameworkCodeRepository
    {
        Task<IEnumerable<string>> GetAsync();
    }
}