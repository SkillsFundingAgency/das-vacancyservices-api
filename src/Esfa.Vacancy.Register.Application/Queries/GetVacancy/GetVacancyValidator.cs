using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public sealed class GetVacancyValidator : AbstractValidator<GetVacancyRequest>

    {
        public GetVacancyValidator()
        {
            RuleFor(request => request.Reference).GreaterThan(0);
        }
    }
}
