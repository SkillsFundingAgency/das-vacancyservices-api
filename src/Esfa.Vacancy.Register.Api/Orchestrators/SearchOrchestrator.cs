using System;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Mappings;
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
            if (apprenticeSearchParameters == null) throw new ArgumentNullException(nameof(apprenticeSearchParameters), "At least one search parameter is required.");

            var request = SearchApprenticeshipVacanciesRequestMapper.Convert(apprenticeSearchParameters);
            var response = await _mediator.Send(request);
            var results = Mapper.Map<SearchResponse<ApprenticeshipSummary>>(response);

            return results;
        }
    }
}
