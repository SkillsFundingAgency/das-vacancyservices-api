using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Domain.Repositories
{
    public interface IStandardRepository
    {
        Task<IEnumerable<StandardSector>> GetStandardsAndRespectiveSectorIdsAsync();
    }
}
