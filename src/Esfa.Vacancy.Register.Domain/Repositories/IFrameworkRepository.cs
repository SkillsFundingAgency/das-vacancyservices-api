using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IFrameworkRepository
    {
        Task<IEnumerable<string>> GetFrameworksAsync();
    }
}