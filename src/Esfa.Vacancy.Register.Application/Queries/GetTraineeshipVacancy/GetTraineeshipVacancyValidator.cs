using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.GetTraineeshipVacancy
{
    public sealed class GetTraineeshipVacancyValidator : AbstractValidator<GetTraineeshipVacancyRequest>

    {
        public GetTraineeshipVacancyValidator()
        {
            RuleFor(request => request.Reference).GreaterThan(0);
        }
    }
}
