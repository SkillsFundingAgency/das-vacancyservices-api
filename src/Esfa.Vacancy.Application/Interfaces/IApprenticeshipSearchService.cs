using System.Threading.Tasks;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface IApprenticeshipSearchService
    {
        Task<SearchApprenticeshipVacanciesResponse> SearchApprenticeshipVacanciesAsync(
            VacancySearchParameters request);
    }
}
