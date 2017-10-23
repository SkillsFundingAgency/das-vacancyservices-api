using Esfa.Vacancy.Register.Domain;
using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy
{
    public sealed class GetApprenticeshipVacancyValidator : AbstractValidator<GetApprenticeshipVacancyRequest>

    {
        public GetApprenticeshipVacancyValidator()
        {
            RuleFor(request => request.Reference)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.GetApprenticeship.VacancyReferenceNumberLessThan0.ToString());
        }
    }
}
