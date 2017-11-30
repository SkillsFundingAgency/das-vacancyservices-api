using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IAsyncRequestHandler<SearchApprenticeshipVacanciesRequest, SearchApprenticeshipVacanciesResponse>
    {
        private readonly IValidator<SearchApprenticeshipVacanciesRequest> _validator;
        private readonly IApprenticeshipSearchService _vacancySearchService;

        public SearchApprenticeshipVacanciesQueryHandler(
            IValidator<SearchApprenticeshipVacanciesRequest> validator,
            IApprenticeshipSearchService vacancySearchService)
        {
            _validator = validator;
            _vacancySearchService = vacancySearchService;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> Handle(SearchApprenticeshipVacanciesRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var searchParameters = VacancySearchParametersMapper.Convert(request);

            return await _vacancySearchService.SearchApprenticeshipVacanciesAsync(searchParameters);
        }
    }
}
