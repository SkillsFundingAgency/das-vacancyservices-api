using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetVacancyRequest : IRequest<GetVacancyResponse>
    {
        public int Reference { get; set; }
    }
}
