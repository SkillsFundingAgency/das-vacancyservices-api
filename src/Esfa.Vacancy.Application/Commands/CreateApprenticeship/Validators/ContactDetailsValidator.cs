using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureContactDetails()
        {
            RuleFor(request => request.ContactName)
                .MaximumLength(100)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactName)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactName);
        }
    }
}