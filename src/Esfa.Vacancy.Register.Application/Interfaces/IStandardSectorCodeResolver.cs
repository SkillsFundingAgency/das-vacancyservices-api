using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IStandardSectorCodeResolver
    {
        Task<List<string>> ResolveAsync(IEnumerable<int> standardIds);
    }
}
