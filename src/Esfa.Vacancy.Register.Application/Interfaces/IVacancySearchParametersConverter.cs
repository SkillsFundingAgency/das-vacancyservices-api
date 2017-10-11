using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Interfaces
{
    public interface IVacancySearchParametersConverter
    {
        Task<VacancySearchParameters> ConvertFrom(SearchApprenticeshipVacanciesRequest request);
    }
}