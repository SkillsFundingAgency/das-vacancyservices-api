using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IFrameworkCodeConverter
    {
        Task<SubCategoryConversionResult> ConvertAsync(IEnumerable<string> frameworksToConvert);
    }
}