using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public interface IVacancyOrchestrator
    {
        Task<Vacancy.Api.Types.Vacancy> GetVacancyDetailsAsync(string id);
    }
}