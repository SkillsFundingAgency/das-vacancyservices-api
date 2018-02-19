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
                .MustBeAValidName()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactName)
                .When(request => request.ContactName != null);

            RuleFor(request => request.ContactEmail)
                .MaximumLength(100)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactEmail)
                .MustBeAValidEmailAddress()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactEmail)
                .When(request => request.ContactEmail != null);

            RuleFor(request => request.ContactNumber)
                .MustBeAValidPhoneNumber()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactNumber)
                .When(request => request.ContactNumber != null);
        }
    }
}