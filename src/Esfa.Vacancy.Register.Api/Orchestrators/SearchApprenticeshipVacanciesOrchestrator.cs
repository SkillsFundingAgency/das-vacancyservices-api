using System;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Validation;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class SearchApprenticeshipVacanciesOrchestrator
    {
        private const string ApprenticeSearchPropertyName = "apprenticeSearchParameters";
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IProvideSettings _provideSettings;
        private readonly IValidationExceptionBuilder _validationExceptionBuilder;

        public SearchApprenticeshipVacanciesOrchestrator(
            IMediator mediator,
            IMapper mapper,
            IProvideSettings provideSettings,
            IValidationExceptionBuilder validationExceptionBuilder)
        {
            _mediator = mediator;
            _mapper = mapper;
            _provideSettings = provideSettings;
            _validationExceptionBuilder = validationExceptionBuilder;
        }

        public async Task<SearchResponse<ApprenticeshipSummary>> SearchApprenticeship(
            SearchApprenticeshipParameters apprenticeSearchParameters, Func<int, string> linkFunc)
        {
            if (apprenticeSearchParameters == null)
            {
                throw _validationExceptionBuilder.Build(
                    ErrorCodes.SearchApprenticeships.SearchApprenticeshipParametersIsNull,
                    ErrorMessages.SearchApprenticeships.SearchApprenticeshipParametersIsNull,
                    ApprenticeSearchPropertyName);
            }

            var request = _mapper.Map<SearchApprenticeshipVacanciesRequest>(apprenticeSearchParameters);
            var response = await _mediator.Send(request).ConfigureAwait(false);
            var results = _mapper.Map<SearchResponse<ApprenticeshipSummary>>(response);


            foreach (ApprenticeshipSummary summary in results.Results)
            {
                summary.VacancyUrl = GetVacancyUrl(summary.VacancyReference);
                summary.ApiDetailUrl = linkFunc(summary.VacancyReference);
            }

            return results;
        }

        private string GetVacancyUrl(int reference)
        {
            string url = _provideSettings.GetSetting(ApplicationSettingKeys.LiveApprenticeshipVacancyBaseUrlKey);
            return url.EndsWith("/") ? $"{url}{reference}" : $"{url}/{reference}";
        }
    }
}
