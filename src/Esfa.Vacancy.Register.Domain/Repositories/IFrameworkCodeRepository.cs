using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IFrameworkCodeRepository
    {
        Task<IEnumerable<string>> GetFrameworksAsync();
    }
}