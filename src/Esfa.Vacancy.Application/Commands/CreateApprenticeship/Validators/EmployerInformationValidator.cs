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
        }
    }
}