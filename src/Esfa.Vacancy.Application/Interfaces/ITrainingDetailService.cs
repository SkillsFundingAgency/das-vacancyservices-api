using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface ITrainingDetailService
    {
        Task<Framework> GetFrameworkDetailsAsync(int code);
        Task<Standard> GetStandardDetailsAsync(int code);
    }
}
