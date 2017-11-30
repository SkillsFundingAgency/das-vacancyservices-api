using Esfa.Vacancy.Register.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy
{
    public sealed class GetTraineeshipVacancyValidator : AbstractValidator<GetTraineeshipVacancyRequest>

    {
        public GetTraineeshipVacancyValidator()
        {
            RuleFor(request => request.Reference)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.GetTraineeship.VacancyReferenceNumberLessThan0);
        }
    }
}
