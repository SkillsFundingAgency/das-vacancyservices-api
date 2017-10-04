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
                .WithMessage((c, t) => $"{t} is invalid, expected a number.");

            RuleFor(r => r.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page size should be greater than or equal to 1")
                .LessThanOrEqualTo(250)
                .WithMessage("Page size should be less than or equal to 250");

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);
        }

        private bool BeValidNumber(string value)
        {
            int i;
            return int.TryParse(value, out i);
        }
    }
}
