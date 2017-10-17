using Entities = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetApprenticeshipVacancyResponse
    {
        public Entities.ApprenticeshipVacancy ApprenticeshipVacancy { get; set; }
    }
}
