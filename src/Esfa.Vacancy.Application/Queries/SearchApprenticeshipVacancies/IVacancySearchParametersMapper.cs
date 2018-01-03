using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public interface IVacancySearchParametersMapper
    {
        VacancySearchParameters Convert(SearchApprenticeshipVacanciesRequest request);
    }
}