using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Validation;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class SearchApprenticeshipVacanciesOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IProvideSettings _provideSettings;

        public SearchApprenticeshipVacanciesOrchestrator(IMediator mediator, IMapper mapper, IProvideSettings provideSettings)
        {
            _mediator = mediator;
            _mapper = mapper;
            _provideSettings = provideSettings;
        }

        public async Task<SearchResponse<ApprenticeshipSummary>> SearchApprenticeship(
            SearchApprenticeshipParameters apprenticeSearchParameters, Func<int, string> linkFunc)
        {
            if (apprenticeSearchParameters == null) ThrowValidationException();

            var request = _mapper.Map<SearchApprenticeshipVacanciesRequest>(apprenticeSearchParameters);
            var response = await _mediator.Send(request);
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
            string url = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveApprenticeshipVacancyBaseUrlKey);
            return url.EndsWith("/") ? $"{url}{reference}" : $"{url}/{reference}";
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
