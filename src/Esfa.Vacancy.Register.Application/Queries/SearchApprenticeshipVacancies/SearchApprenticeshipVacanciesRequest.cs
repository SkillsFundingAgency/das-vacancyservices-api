using System.Collections.Generic;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequest : IRequest<SearchApprenticeshipVacanciesResponse>
    {
        public IEnumerable<string> StandardCodes { get; set; }
        public IEnumerable<string> FrameworkCodes { get; set; }
        public int PageSize { get; set; } = 100;
        public int PageNumber { get; set; } = 1;
    }
}
