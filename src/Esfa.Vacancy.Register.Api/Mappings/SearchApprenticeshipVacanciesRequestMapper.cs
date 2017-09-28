using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public static class SearchApprenticeshipVacanciesRequestMapper
    {
        public static SearchApprenticeshipVacanciesRequest Convert(SearchApprenticeshipParameters source)
        {
            var result = new SearchApprenticeshipVacanciesRequest();

            result.StandardCodes = GetConvertedStandardCodes(source.StandardCodes);

            return result;
        }

        private static IEnumerable<string> GetConvertedStandardCodes(string codes)
        {
            if (string.IsNullOrEmpty(codes)) return null;
            var i = 0;
            return codes.Split(',')
                .Where(code => !string.IsNullOrWhiteSpace(code) && int.TryParse(code, out i))
                .Select(code => $"{ApiConstants.Training.StandardSectorPrefix}.{code.Trim()}");
        }
    }
}
