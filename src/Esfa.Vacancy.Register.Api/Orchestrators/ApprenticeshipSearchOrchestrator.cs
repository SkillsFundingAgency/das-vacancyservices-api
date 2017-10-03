using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class ApprenticeshipSearchOrchestrator : ISearchOrchestrator
    {
        private readonly IMediator _mediator;

        public ApprenticeshipSearchOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<SearchResponse<ApprenticeshipSummary>> SearchApprenticeship(
            SearchApprenticeshipParameters apprenticeSearchParameters)
        {
            if (apprenticeSearchParameters == null) throw new ValidationException("At least one search parameter is required.");
            var request = Mapper.Map<SearchApprenticeshipVacanciesRequest>(apprenticeSearchParameters);
            var response = await _mediator.Send(request);
            var results = Mapper.Map<SearchResponse<ApprenticeshipSummary>>(response);
            return results;
        }
    }
}
