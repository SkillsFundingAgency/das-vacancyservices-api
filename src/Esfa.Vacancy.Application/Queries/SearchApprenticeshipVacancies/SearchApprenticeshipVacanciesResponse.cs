using System.Collections.Generic;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesResponse
    {
        public long TotalMatched { get; set; }

        public long TotalReturned { get; set; }

        public IEnumerable<ApprenticeshipSummary> ApprenticeshipSummaries { get; set; }
        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }
    }
}
