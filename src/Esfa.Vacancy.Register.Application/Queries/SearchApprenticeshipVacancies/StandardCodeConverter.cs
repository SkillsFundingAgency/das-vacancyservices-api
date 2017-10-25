using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentValidation.Results;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class StandardCodeConverter : IStandardCodeConverter
    {
        private readonly IStandardRepository _standardRepository;

        public StandardCodeConverter(IStandardRepository standardRepository)
        {
            _standardRepository = standardRepository;
        }

        public async Task<SubCategoryConversionResult> ConvertToSearchableCodesAsync(IList<string> standards)
        {
            var result = new SubCategoryConversionResult();

            if (!standards.Any())
                return result;

            var standardSectorIds = (await _standardRepository.GetStandardsAndRespectiveSectorIdsAsync()).ToList();

            var convertedStandardCodes = new HashSet<string>();

            foreach (var standardToConvert in standards)
            {
                var parsedStandardToConvert = int.Parse(standardToConvert);

                var standardSector = standardSectorIds.FirstOrDefault(ss => ss.LarsCode == parsedStandardToConvert);
                if (standardSector == null)
                {
                    result.ValidationFailures
                        .Add(new ValidationFailure("StandardCode", string.Format(ErrorMessages.SearchApprenticeships.StandardCodeNotFound, parsedStandardToConvert))
                        { ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotFound });
                }
                else
                {
                    convertedStandardCodes.Add($"{StandardSector.StandardSectorPrefix}.{standardSector.StandardSectorId}");
                }
            }

            result.SubCategoryCodes.AddRange(convertedStandardCodes);

            return result;
        }
    }
}