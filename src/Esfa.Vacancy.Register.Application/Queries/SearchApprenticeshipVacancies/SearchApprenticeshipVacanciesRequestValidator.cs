using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequestValidator : AbstractValidator<SearchApprenticeshipVacanciesRequest>
    {
        private readonly IFrameworkCodeRepository _frameworkCodeRepository;
        private readonly IStandardRepository _standardRepository;
        private const string MinimumFieldsErrorMessage = "At least one of StandardCodes or FrameworkCodes is required.";
        private const int MinimumPageSize = 1;
        private const int MinimumPageNumber = 1;
        private const int MaximumPageSize = 250;

        public SearchApprenticeshipVacanciesRequestValidator(
            IFrameworkCodeRepository frameworkCodeRepository,
            IStandardRepository standardRepository)
        {
            _frameworkCodeRepository = frameworkCodeRepository;
            _standardRepository = standardRepository;

            RuleFor(request => request.StandardCodes)
                .NotEmpty()
                .When(request => !request.FrameworkCodes.Any())
                .WithMessage(MinimumFieldsErrorMessage);

            RuleForEach(request => request.StandardCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => $"{t} is invalid, expected a number.")
                .DependentRules(d => d.RuleForEach(request => request.StandardCodes)
                    .MustAsync(BeAValidStandardCode)
                    .WithMessage((c, value) => $"Standard code {value} is invalid."));

            RuleForEach(request => request.FrameworkCodes)
                .Must(BeValidNumber)
                .WithMessage((c, t) => $"{t} is invalid, expected a number.")
                .DependentRules(d => d.RuleForEach(request => request.FrameworkCodes)
                    .MustAsync(BeAValidFrameworkCode)
                    .WithMessage((c, value) => $"Framework code {value} is invalid."));

            RuleFor(r => r.PageSize)
                .GreaterThanOrEqualTo(MinimumPageSize)
                .LessThanOrEqualTo(MaximumPageSize);

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(MinimumPageNumber);

            RuleFor(r => r.PostedInLastNumberOfDays)
                .GreaterThanOrEqualTo(0);
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

        private async Task<bool> BeAValidStandardCode(string standardCode, CancellationToken token)
        {
            var validStandards = (await _standardRepository.GetStandardIdsAsync()).ToList();

            return validStandards.Any(larsCode =>
                larsCode.Equals(int.Parse(standardCode)));
        }
    }
}
