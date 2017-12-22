using Esfa.Vacancy.Domain.Entities;
using Nest;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public interface IGeoSearchResultDistanceSetter
    {
        void SetDistance(VacancySearchParameters parameters, ISearchResponse<ApprenticeshipSummary> response);
    }
}