using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class SearchApprenticeshipVacanciesOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SearchApprenticeshipVacanciesOrchestrator(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<SearchResponse<ApprenticeshipSummary>> SearchApprenticeship(
            SearchApprenticeshipParameters apprenticeSearchParameters, UrlHelper urlHelper)
        {
            if (apprenticeSearchParameters == null) ThrowValidationException();

            var request = _mapper.Map<SearchApprenticeshipVacanciesRequest>(apprenticeSearchParameters);
            var response = await _mediator.Send(request);
            var results = _mapper.Map<SearchResponse<ApprenticeshipSummary>>(response);

            foreach (ApprenticeshipSummary summary in results.Results)
            {
                summary.ApiDetailUrl = urlHelper.Link("GetApprenticeshipVacancyByReference", new { vacancyReference = summary.VacancyReference });
            }

            return results;
        }

        private static void ThrowValidationException()
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new ValidationFailure("apprenticeSearchParameters", ErrorMessages.SearchApprenticeships.SearchApprenticeshipParametersIsNull)
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.SearchApprenticeshipParametersIsNull
                    }
                });
        }
    }
}
