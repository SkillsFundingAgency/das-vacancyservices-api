using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Interfaces
{
    public interface ICreateApprenticeshipService
    {
        Task<int> CreateApprenticeshipAsync(CreateApprenticeshipParameters parameters);
    }
}