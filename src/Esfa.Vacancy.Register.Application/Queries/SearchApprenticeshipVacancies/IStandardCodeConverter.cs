using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public interface IStandardCodeConverter
    {
        Task<SubCategoryConversionResult> ConvertToSearchableCodesAsync(IList<string> standards);
    }
}