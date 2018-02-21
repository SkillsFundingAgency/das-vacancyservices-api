using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface ICachedTrainingDetailService
    {
        Task<Framework> GetFrameworkDetailsAsync(int code);
        Task<Standard> GetStandardDetailsAsync(int code);
        Task<TrainingDetail> GetFrameworkDetailsAsync(string frameworkCode);
        Task<TrainingDetail> GetStandardDetailsAsync(string standardLarsCode);
    }
}