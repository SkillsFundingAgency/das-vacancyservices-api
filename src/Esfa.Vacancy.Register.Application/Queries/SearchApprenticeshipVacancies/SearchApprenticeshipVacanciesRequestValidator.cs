using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequestValidator : AbstractValidator<SearchApprenticeshipVacanciesRequest>
    {
        private readonly IFrameworkCodeRepository _frameworkCodeRepository;
        private const int MinimumPageSize = 1;
        private const int MinimumPageNumber = 1;
        private const int MaximumPageSize = 250;

        public SearchApprenticeshipVacanciesRequestValidator(
            IFrameworkCodeRepository frameworkCodeRepository)
        {
            _frameworkCodeRepository = frameworkCodeRepository;

            RuleFor(request => request.StandardCodes)
                .NotEmpty()
                .When(request => !request.FrameworkCodes.Any())
                .WithMessage(ErrorMessages.SearchApprenticeships.StandardAndFrameworkCodeNotProvided)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardAndFrameworkCodeNotProvided);

            RuleForEach(request => request.StandardCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => string.Format(ErrorMessages.SearchApprenticeships.StandardCodeNotInt32, t))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardCodeNotInt32);

            RuleForEach(request => request.FrameworkCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => string.Format(ErrorMessages.SearchApprenticeships.FrameworkCodeNotInt32, t))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .DependentRules(d => d.RuleForEach(request => request.FrameworkCodes)
                    .MustAsync(BeAValidFrameworkCode)
                    .WithMessage((c, value) => $"Framework code {value} is invalid."));

            RuleFor(r => r.PageSize)
                .InclusiveBetween(MinimumPageSize, MaximumPageSize)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageSizeOutsideRange);

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

        private async Task<bool> BeAValidFrameworkCode(string frameworkCode, CancellationToken token)
        {
            var validFrameworks = (await _frameworkCodeRepository.GetAsync()).ToList();

            return validFrameworks.Any(larsCode =>
                larsCode.Equals(frameworkCode.Trim(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
