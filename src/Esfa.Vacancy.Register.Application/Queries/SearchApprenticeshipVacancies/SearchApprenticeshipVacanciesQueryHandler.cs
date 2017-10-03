using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesQueryHandler : IAsyncRequestHandler<SearchApprenticeshipVacanciesRequest, SearchApprenticeshipVacanciesResponse>
    {
        private readonly IValidator<SearchApprenticeshipVacanciesRequest> _validator;
        private readonly IVacancySearchService _vacancySearchService;
        private readonly IStandardRepository _standardRepository;

        public SearchApprenticeshipVacanciesQueryHandler(
            IValidator<SearchApprenticeshipVacanciesRequest> validator,
            IVacancySearchService vacancySearchService,
            IStandardRepository standardRepository)
        {
            _validator = validator;
            _vacancySearchService = vacancySearchService;
            _standardRepository = standardRepository;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> Handle(SearchApprenticeshipVacanciesRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var searchParameters = new VacancySearchParameters
            {
                StandardSectorCodes = await ValidateStandardCodes(request.StandardCodes.Select(int.Parse))
            };

            var result = await _vacancySearchService.SearchApprenticeshipVacanciesAsync(searchParameters);
            return result;
        }

        private async Task<List<string>> ValidateStandardCodes(IEnumerable<int> standardCodes)
        {
            var standardSectorIds = await _standardRepository.GetStandardsAndRespectiveSectorIdsAsync();

            var errors = new List<ValidationFailure>();
            var sectorCodes = new List<string>();

            standardCodes.ToList().ForEach(standardCode =>
            {
                var standardSector = standardSectorIds.FirstOrDefault(ss => ss.LarsCode == standardCode);
                if (standardSector == null)
                {
                    errors.Add(new ValidationFailure("StandardCode", $"StandardCode {standardCode} is invalid"));
                }
                else
                {
                    sectorCodes.Add($"{StandardSector.StandardSectorPrefix}.{standardSector.StandardSectorId}");
                }
            });

            if (errors.Any())
                throw new ValidationException(errors);

            return sectorCodes;
        }
    }
}
