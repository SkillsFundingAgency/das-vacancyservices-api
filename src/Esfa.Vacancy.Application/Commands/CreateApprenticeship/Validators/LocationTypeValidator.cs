using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureLocationTypeValidator()
        {
            RuleFor(request => request.LocationType)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.LocationTypeIsRequired);
        }
    }
}