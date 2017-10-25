using System.Linq;
using Esfa.Vacancy.Register.Domain.Validation;
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
            RuleFor(request => request.StandardCodes)
                .NotEmpty()
                .When(request => request.FrameworkCodes == null || !request.FrameworkCodes.Any())
                .WithMessage(ErrorMessages.SearchApprenticeships.StandardAndFrameworkCodeNotProvided)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardAndFrameworkCodeNotProvided);

            RuleForEach(request => request.StandardCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => string.Format(ErrorMessages.SearchApprenticeships.StandardCodeNotInt32, t))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardCodeNotInt32);

            RuleForEach(request => request.FrameworkCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => string.Format(ErrorMessages.SearchApprenticeships.FrameworkCodeNotInt32, t))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32);

            RuleFor(r => r.PageSize)
                .GreaterThanOrEqualTo(MinimumPageSize)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageSizeLessThan1)
                .LessThanOrEqualTo(MaximumPageSize)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageSizeGreaterThan250);

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(MinimumPageNumber)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageNumberLessThan1);

            RuleFor(r => r.PostedInLastNumberOfDays)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PostedInLastNumberOfDaysLessThan0);
        }

        private static bool BeValidNumber(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }
    }
}
