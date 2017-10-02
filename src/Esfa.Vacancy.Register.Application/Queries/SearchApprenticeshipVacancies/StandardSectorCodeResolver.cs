using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Repositories;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class StandardSectorCodeResolver : IStandardSectorCodeResolver
    {
        public const string StandardSectorPrefix = "STDSEC";

        private readonly IStandardRepository _standardRepository;

        public StandardSectorCodeResolver(IStandardRepository standardRepository)
        {
            _standardRepository = standardRepository;
        }

        public async Task<List<string>> ResolveAsync(IEnumerable<int> standardIds)
        {
            var standardSectorIds =
                await _standardRepository.GetDistinctStandardSectorIdsAsync(standardIds);
            var result = new List<string>();

            foreach (var standardSectorId in standardSectorIds)
            {
                result.Add($"{StandardSectorCodeResolver.StandardSectorPrefix}.{standardSectorId}");
            }

            return result;
        }
    }
}
