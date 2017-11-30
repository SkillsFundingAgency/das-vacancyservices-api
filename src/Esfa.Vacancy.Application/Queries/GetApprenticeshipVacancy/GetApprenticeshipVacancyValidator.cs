using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy
{
    public sealed class GetApprenticeshipVacancyValidator : AbstractValidator<GetApprenticeshipVacancyRequest>

    {
        public GetApprenticeshipVacancyValidator()
        {
            RuleFor(request => request.Reference)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.GetApprenticeship.VacancyReferenceNumberLessThan0);
        }
    }
}
