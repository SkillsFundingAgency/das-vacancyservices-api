using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IAsyncRequestHandler<SearchApprenticeshipVacanciesRequest, SearchApprenticeshipVacanciesResponse>
    {
        private readonly IValidator<SearchApprenticeshipVacanciesRequest> _validator;
        private readonly IApprenticeshipSearchService _vacancySearchService;
        private readonly IVacancySearchParametersConverter _vacancySearchParametersConverter;

        public SearchApprenticeshipVacanciesQueryHandler(
            IValidator<SearchApprenticeshipVacanciesRequest> validator,
            IApprenticeshipSearchService vacancySearchService,
            IVacancySearchParametersConverter vacancySearchParametersConverter)
        {
            _validator = validator;
            _vacancySearchService = vacancySearchService;
            _vacancySearchParametersConverter = vacancySearchParametersConverter;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> Handle(SearchApprenticeshipVacanciesRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var searchParameters = await _vacancySearchParametersConverter.ConvertFrom(request);

            return await _vacancySearchService.SearchApprenticeshipVacanciesAsync(searchParameters);
        }
    }
}
