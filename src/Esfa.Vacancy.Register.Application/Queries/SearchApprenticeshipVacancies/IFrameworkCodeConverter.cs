using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public interface IFrameworkCodeConverter
    {
        Task<SubCategoryConversionResult> ConvertToSearchableCodesAsync(IList<string> frameworks);
    }
}