using System;
using Entities = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetApprenticeshipVacancyResponse
    {
        public Entities.Vacancy Vacancy { get; set; }
    }
}
