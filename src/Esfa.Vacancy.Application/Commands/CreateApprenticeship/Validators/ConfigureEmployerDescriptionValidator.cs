using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureEmployerDescriptionValidator()
        {
            RuleFor(request => request.EmployerDescription)
                .MaximumLength(4000)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerDescription)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerDescription);

        }
    }
}