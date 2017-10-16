using System.Linq;
using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequestValidator : AbstractValidator<SearchApprenticeshipVacanciesRequest>
    {
        private const string MinimumFieldsErrorMessage = "At least one of StandardCodes or FrameworkCodes is required.";
        private const int MinimumPageSize = 1;
        private const int MinimumPageNumber = 1;
        private const int MaximumPageSize = 250;

        public SearchApprenticeshipVacanciesRequestValidator()
        {
            RuleFor(request => request.StandardCodes)
                .NotEmpty()
                .When(request => !request.FrameworkCodes.Any())
                .WithMessage(MinimumFieldsErrorMessage);

            RuleForEach(request => request.StandardCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => $"{t} is invalid, expected a number.");

            RuleForEach(request => request.FrameworkCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => $"{t} is invalid, expected a number.");

            RuleFor(r => r.PageSize)
                .GreaterThanOrEqualTo(MinimumPageSize)
                .LessThanOrEqualTo(MaximumPageSize);

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(MinimumPageNumber);
        }

        private static bool BeValidNumber(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }
    }
}
