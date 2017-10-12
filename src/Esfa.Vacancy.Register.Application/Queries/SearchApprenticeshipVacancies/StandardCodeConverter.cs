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
    public class StandardCodeConverter : IStandardCodeConverter
    {
        private readonly IStandardRepository _standardRepository;

        public StandardCodeConverter(IStandardRepository standardRepository)
        {
            _standardRepository = standardRepository;
        }

        public async Task<List<string>> Convert(IEnumerable<string> standardsToConvert)
        {
            var standardSectorIds = await _standardRepository.GetStandardsAndRespectiveSectorIdsAsync();

            var errors = new List<ValidationFailure>();
            var sectorCodes = new HashSet<string>();

            standardsToConvert.ToList().ForEach(standardToConvert =>
            {
                var parsedStandardToConvert = int.Parse(standardToConvert);

                var standardSector = standardSectorIds.FirstOrDefault(ss => ss.LarsCode == parsedStandardToConvert);
                if (standardSector == null)
                {
                    errors.Add(new ValidationFailure("StandardCode", $"StandardCode {standardToConvert} is invalid"));
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