using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IVacancySearchService
    {
        Task<SearchApprenticeshipVacanciesResponse> SearchApprenticeshipVacancies(
            SearchApprenticeshipVacanciesRequest request);
    }
}
