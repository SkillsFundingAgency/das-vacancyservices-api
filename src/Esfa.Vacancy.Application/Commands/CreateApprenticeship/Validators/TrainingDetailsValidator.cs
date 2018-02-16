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

            RuleFor(request => request.TrainingCode)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode)
                .Must(MustBeAcceptableStandardCode)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode)
                .WithMessage(ErrorMessages.CreateApprenticeship.InvalidStandardLarsCode);
        }

        private static bool MustBeAcceptableStandardCode(string code)
        {
            if (int.TryParse(code, out var validCode))
            {
                return validCode > 0 && validCode <= 9999;
            }
            return false;
        }
    }
}
