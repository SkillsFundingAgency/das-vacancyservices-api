using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IAsyncRequestHandler<SearchApprenticeshipVacanciesRequest, SearchApprenticeshipVacanciesResponse>
    {
        private readonly IVacancySearchService _vacancySearchService;

        public SearchApprenticeshipVacanciesQueryHandler(IVacancySearchService vacancySearchService)
        {
            _vacancySearchService = vacancySearchService;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> Handle(SearchApprenticeshipVacanciesRequest request)
        {
            var result = await _vacancySearchService.SearchApprenticeshipVacancies(request);
            return result;
        }
    }
}
