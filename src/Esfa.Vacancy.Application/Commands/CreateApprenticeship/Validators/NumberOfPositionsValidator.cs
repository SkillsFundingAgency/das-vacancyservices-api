using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureNumberOfPositions()
        {
            const int maxPositionsAllowed = 5000;
            RuleFor(request => request.NumberOfPositions)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.NumberOfPositions)
                .InclusiveBetween(1, maxPositionsAllowed)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.NumberOfPositions);
        }
    }
}