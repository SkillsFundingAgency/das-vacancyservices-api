using System.Collections.Generic;
using Esfa.Vacancy.Register.Domain.Entities;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequest : IRequest<SearchApprenticeshipVacanciesResponse>
    {
        public List<string> StandardLarsCodes { get; set; } = new List<string>();
        public List<string> FrameworkLarsCodes { get; set; } = new List<string>();
        public int PageSize { get; set; } = 100;
        public int PageNumber { get; set; } = 1;
        public int? PostedInLastNumberOfDays { get; set; }
        public bool NationwideOnly { get; set; }
        public double? Latitude { get; set; }
    }
}
