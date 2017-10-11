using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetTraineeshipVacancyRequest : IRequest<GetTraineeshipVacancyResponse>
    {
        public int Reference { get; set; }
    }
}