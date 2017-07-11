using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Domain.Interfaces
{
    public interface ITrainingDetailService
    {
        Task<Framework> GetFrameworkDetails(int code);
        Task<Standard> GetStandardDetails(int code);
    }
}
