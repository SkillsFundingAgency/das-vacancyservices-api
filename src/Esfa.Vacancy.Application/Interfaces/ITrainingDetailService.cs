using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface ITrainingDetailService
    {
        Task<Framework> GetFrameworkDetailsAsync(int code);
        Task<IEnumerable<TrainingDetail>> GetAllFrameworkDetailsAsync();
        Task<IEnumerable<TrainingDetail>> GetAllStandardDetailsAsync();

        Task<Standard> GetStandardDetailsAsync(int code);
    }
}
