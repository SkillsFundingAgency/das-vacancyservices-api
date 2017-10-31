using System.Collections.Generic;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequest : IRequest<SearchApprenticeshipVacanciesResponse>
    {
        public List<string> StandardCodes { get; set; } = new List<string>();
        public List<string> FrameworkCodes { get; set; } = new List<string>();
        public int PageSize { get; set; } = 100;
        public int PageNumber { get; set; } = 1;
        public int? PostedInLastNumberOfDays { get; set; }
    }
}
