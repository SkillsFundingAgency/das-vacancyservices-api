using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetTraineeshipVacancy
{
    public sealed class GetTraineeshipVacancyRequest : IRequest<GetTraineeshipVacancyResponse>
    {
        public int Reference { get; set; }
    }
}