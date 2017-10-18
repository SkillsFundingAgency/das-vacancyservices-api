using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy
{
    public sealed class GetApprenticeshipVacancyRequest : IRequest<GetApprenticeshipVacancyResponse>
    {
        public int Reference { get; set; }
    }
}
