using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureKeyIdentifiers()
        {
            RuleFor(request => request.ProviderUkprn)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ProviderUkprn)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ProviderUkprn);
            RuleFor(request => request.EmployerEdsUrn)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerEdsUrn)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.EmployerEdsUrn);
            RuleFor(request => request.ProviderSiteEdsUrn)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ProviderSiteEdsUrn)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.ProviderSiteEdsUrn);
        }
    }
}
