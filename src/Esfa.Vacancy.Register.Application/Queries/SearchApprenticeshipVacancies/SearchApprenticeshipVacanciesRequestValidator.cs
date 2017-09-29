using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequestValidator : AbstractValidator<SearchApprenticeshipVacanciesRequest>
    {
        public SearchApprenticeshipVacanciesRequestValidator()
        {
            RuleFor(r => r.StandardCodes)
                .NotNull()
                .WithMessage("At least one search parameter is required.");

            RuleForEach(r => r.StandardCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => $"{t} is invalid, expected a number");
        }

        private bool BeValidNumber(string value)
        {
            int i;
            return int.TryParse(value, out i);
        }
    }
}
