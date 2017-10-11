using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class VacancySearchParametersConverter : IVacancySearchParametersConverter
    {
        private readonly IStandardRepository _standardRepository;
        private readonly IFrameworkCodeConverter _frameworkCodeConverter;

        public VacancySearchParametersConverter(IStandardRepository standardRepository, IFrameworkCodeConverter frameworkCodeConverter)
        {
            _standardRepository = standardRepository;
            _frameworkCodeConverter = frameworkCodeConverter;
        }

        public async Task<VacancySearchParameters> ConvertFrom(SearchApprenticeshipVacanciesRequest request)
        {
            var combinedSubCategoryCodes = new List<string>();
            combinedSubCategoryCodes.AddRange(await ConvertStandardCodesToSearchableSectorCodes(request.StandardCodes.Select(int.Parse)));
            combinedSubCategoryCodes.AddRange(await _frameworkCodeConverter.Convert(request.FrameworkCodes));

            return new VacancySearchParameters
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SubCategoryCodes = combinedSubCategoryCodes
            };
        }

        private async Task<List<string>> ConvertStandardCodesToSearchableSectorCodes(IEnumerable<int> standardCodes)
        {
            var standardSectorIds = await _standardRepository.GetStandardsAndRespectiveSectorIdsAsync();

            var errors = new List<ValidationFailure>();
            var sectorCodes = new HashSet<string>();

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

            return sectorCodes.ToList();
        }
    }
}
