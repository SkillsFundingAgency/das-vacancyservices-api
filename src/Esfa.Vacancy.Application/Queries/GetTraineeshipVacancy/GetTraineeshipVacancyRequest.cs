using MediatR;

namespace Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy
{
    public sealed class GetTraineeshipVacancyRequest : IRequest<GetTraineeshipVacancyResponse>
    {
        public int Reference { get; set; }
    }
}