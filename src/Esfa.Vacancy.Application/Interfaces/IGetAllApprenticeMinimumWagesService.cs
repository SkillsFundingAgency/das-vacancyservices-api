using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface IGetAllApprenticeMinimumWagesService
    {
        Task<IEnumerable<WageRange>> GetAllWagesAsync();
    }
}