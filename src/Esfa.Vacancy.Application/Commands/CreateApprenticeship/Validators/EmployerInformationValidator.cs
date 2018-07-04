using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureEmployerInformationValidator()
        {
            RuleFor(req => req.IsEmployerDisabilityConfident)
                .NotNull()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.IsEmployerDisabilityConfident);

            RuleFor(request => request.EmployerDescription)
                .MaximumLength(4000)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerDescription)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerDescription);

            RuleFor(request => request.EmployerWebsiteUrl)
                .MustBeAValidWebUrl()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerWebsite)
                .When(r => !string.IsNullOrEmpty(r.EmployerWebsiteUrl));
        }
    }
}