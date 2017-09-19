using System;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetVacancyRequest : IRequest<GetVacancyResponse>
    {
        public string Reference { get; set; }
    }
}
