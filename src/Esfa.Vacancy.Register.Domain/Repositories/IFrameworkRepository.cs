using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IFrameworkRepository
    {
        Task<IEnumerable<Framework>> GetFrameworksAsync(); //todo: investigate if need new return type
    }
}