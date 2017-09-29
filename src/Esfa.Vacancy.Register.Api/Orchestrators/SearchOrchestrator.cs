using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class SearchOrchestrator : ISearchOrchestrator
    {
        private readonly IMediator _mediator;

        public SearchOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<SearchResponse<ApprenticeshipSummary>> SearchApprenticeship(
            SearchApprenticeshipParameters apprenticeSearchParameters)
        {
            var request = Mapper.Map<SearchApprenticeshipVacanciesRequest>(apprenticeSearchParameters);
            var response = await _mediator.Send(request);
            var results = Mapper.Map<SearchResponse<ApprenticeshipSummary>>(response);
            return results;
        }
    }
}
