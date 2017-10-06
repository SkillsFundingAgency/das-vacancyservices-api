using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequestValidator : AbstractValidator<SearchApprenticeshipVacanciesRequest>
    {
        private const int MinimumPageSize = 1;
        private const int MinimumPageNumber = 1;
        private const int MaximumPageSize = 250;
        public SearchApprenticeshipVacanciesRequestValidator()
        {
            RuleFor(r => r.StandardCodes)
                .NotNull()
                .WithMessage("At least one search parameter is required.");

            RuleForEach(r => r.StandardCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => $"{t} is invalid, expected a number.");

            RuleFor(r => r.PageSize)
                .GreaterThanOrEqualTo(MinimumPageSize)
                .WithMessage("Page size should be greater than or equal to 1")
                .LessThanOrEqualTo(MaximumPageSize)
                .WithMessage("Page size should be less than or equal to 250");

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(MinimumPageNumber);
        }

        private bool BeValidNumber(string value)
        {
            int i;
            return int.TryParse(value, out i);
        }
    }
}
