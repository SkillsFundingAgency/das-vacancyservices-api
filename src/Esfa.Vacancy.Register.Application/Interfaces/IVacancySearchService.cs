using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IVacancySearchService
    {
        Task<List<ApprenticeshipSummary>> SearchApprenticeshipVacancies(
            SearchApprenticeshipVacanciesRequest request);
    }
}
