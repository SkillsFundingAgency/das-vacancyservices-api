using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ValidateLocationType()
        {
            RuleFor(request => request.LocationType)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.LocationTypeIsRequired);
        }
    }
}