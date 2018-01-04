using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public interface ISortByCalculator
    {
        SortBy CalculateSortBy(SearchApprenticeshipVacanciesRequest request);
    }
}