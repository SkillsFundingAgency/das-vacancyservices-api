using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SortByCalculator : ISortByCalculator
    {
        public SortBy CalculateSortBy(SearchApprenticeshipVacanciesRequest request)
        {
            if (request.SortBy.HasValue)
                return request.SortBy.Value;

            return request.IsGeoSearch ? SortBy.Distance : SortBy.Age;
        }
    }
}