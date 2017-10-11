using System;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetApprenticeshipVacancyRequest : IRequest<GetApprenticeshipVacancyResponse>
    {
        public int Reference { get; set; }
    }
}
