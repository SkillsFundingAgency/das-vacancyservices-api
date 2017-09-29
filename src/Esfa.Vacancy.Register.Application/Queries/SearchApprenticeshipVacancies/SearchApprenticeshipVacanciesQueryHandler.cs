using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IAsyncRequestHandler<SearchApprenticeshipVacanciesRequest, SearchApprenticeshipVacanciesResponse>
    {
        private readonly AbstractValidator<SearchApprenticeshipVacanciesRequest> _validator;
        private readonly IVacancySearchService _vacancySearchService;

        public SearchApprenticeshipVacanciesQueryHandler(
            AbstractValidator<SearchApprenticeshipVacanciesRequest> validator,
            IVacancySearchService vacancySearchService)
        {
            _validator = validator;
            _vacancySearchService = vacancySearchService;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> Handle(SearchApprenticeshipVacanciesRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var result = await _vacancySearchService.SearchApprenticeshipVacancies(request);
            return result;
        }
    }
}
