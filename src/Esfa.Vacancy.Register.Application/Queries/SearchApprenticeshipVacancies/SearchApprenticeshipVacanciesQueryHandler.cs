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
        private readonly IVacancySearchParametersBuilder _vacancySearchParametersBuilder;

        public SearchApprenticeshipVacanciesQueryHandler(
            IValidator<SearchApprenticeshipVacanciesRequest> validator,
            IApprenticeshipSearchService vacancySearchService,
            IVacancySearchParametersBuilder vacancySearchParametersBuilder)
        {
            _validator = validator;
            _vacancySearchService = vacancySearchService;
            _vacancySearchParametersBuilder = vacancySearchParametersBuilder;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> Handle(SearchApprenticeshipVacanciesRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var searchParameters = await _vacancySearchParametersBuilder.BuildAsync(request);

            return await _vacancySearchService.SearchApprenticeshipVacanciesAsync(searchParameters);
        }
    }
}
