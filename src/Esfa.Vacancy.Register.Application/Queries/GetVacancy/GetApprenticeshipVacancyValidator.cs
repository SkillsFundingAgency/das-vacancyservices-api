using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetApprenticeshipVacancyValidator : AbstractValidator<GetApprenticeshipVacancyRequest>

    {
        public GetApprenticeshipVacancyValidator()
        {
            RuleFor(request => request.Reference).GreaterThan(0);
        }
    }
}
