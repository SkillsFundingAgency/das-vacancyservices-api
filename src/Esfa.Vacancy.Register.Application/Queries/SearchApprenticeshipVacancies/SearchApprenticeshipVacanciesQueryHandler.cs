using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IAsyncRequestHandler<SearchApprenticeshipVacanciesRequest, SearchApprenticeshipVacanciesResponse>
    {
        private readonly IValidator<SearchApprenticeshipVacanciesRequest> _validator;
        private readonly IVacancySearchService _vacancySearchService;
        private readonly IStandardSectorCodeResolver _standardSectorCodeResolver;

        public SearchApprenticeshipVacanciesQueryHandler(
            IValidator<SearchApprenticeshipVacanciesRequest> validator,
            IVacancySearchService vacancySearchService,
            IStandardSectorCodeResolver standardSectorCodeResolver)
        {
            _validator = validator;
            _vacancySearchService = vacancySearchService;
            _standardSectorCodeResolver = standardSectorCodeResolver;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> Handle(SearchApprenticeshipVacanciesRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var standardIds = request.StandardCodes.Select(int.Parse);

            var searchParameters = new VacancySearchParameters();

            searchParameters.StandardSectorCodes = await _standardSectorCodeResolver.ResolveAsync(standardIds);

            var result = await _vacancySearchService.SearchApprenticeshipVacanciesAsync(searchParameters);
            return result;
        }
    }
}
