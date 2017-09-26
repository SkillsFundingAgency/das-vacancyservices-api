using System.Collections.Generic;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesResponse
    {
        public List<ApprenticeshipSummary> ApprenticeshipSummaries { get; set; }
    }
}
