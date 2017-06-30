using System;
using Entities = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetVacancyResponse
    {
        public Entities.Vacancy Vacancy { get; set; }
    }
}
