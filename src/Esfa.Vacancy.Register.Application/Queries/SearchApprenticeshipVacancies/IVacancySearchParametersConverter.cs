using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public interface IVacancySearchParametersConverter
    {
        Task<VacancySearchParameters> ConvertFrom(SearchApprenticeshipVacanciesRequest request);
    }
}