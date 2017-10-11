using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetTraineeshipVacancyValidator : AbstractValidator<GetTraineeshipVacancyRequest>

    {
        public GetTraineeshipVacancyValidator()
        {
            RuleFor(request => request.Reference).GreaterThan(0);
        }
    }
}
