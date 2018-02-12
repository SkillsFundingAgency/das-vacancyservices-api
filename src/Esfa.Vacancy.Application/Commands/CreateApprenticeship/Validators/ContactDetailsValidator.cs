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
                .When(request => string.IsNullOrWhiteSpace(request.ContactName) == false);

            RuleFor(request => request.ContactEmail)
                .MaximumLength(100)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactEmail)
                .MustBeAValidEmailAddress()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactEmail)
                .When(request => string.IsNullOrWhiteSpace(request.ContactEmail) == false);

            RuleFor(request => request.ContactNumber)
                .MustBeAValidPhoneNumber()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ContactNumber)
                .When(request => string.IsNullOrWhiteSpace(request.ContactNumber) == false);
        }
    }
}