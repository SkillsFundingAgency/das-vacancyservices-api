using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureTrainingDetailsValidator()
        {
            RuleFor(request => request.TrainingType)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingType);
        }
    }
}
