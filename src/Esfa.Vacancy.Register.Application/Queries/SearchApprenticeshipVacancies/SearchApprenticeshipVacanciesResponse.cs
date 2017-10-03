using System.Collections.Generic;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesResponse
    {
        public long TotalMatched { get; set; }

        public long TotalReturned { get; set; }

        public IEnumerable<ApprenticeshipSummary> ApprenticeshipSummaries { get; set; }
    }
}
