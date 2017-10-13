using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IStandardCodeConverter
    {
        Task<SubCategoryConversionResult> ConvertAsync(List<string> standardsToConvert);
    }
}