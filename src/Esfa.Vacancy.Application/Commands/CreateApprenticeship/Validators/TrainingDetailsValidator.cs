using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const string RegexFrameworkCode = @"^\d{1,4}-\d{1,2}-\d{1,2}$";

        private void ConfigureTrainingDetailsValidator()
        {
            RuleFor(request => request.TrainingType)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingType);

            RuleFor(request => request.TrainingCode)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode);

            RuleFor(request => request.IsTrainingCodeValid)
                .Equal(true)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode)
                .WithName("Training Code")
                .WithMessage(ErrorMessages.CreateApprenticeship.InvalidPropertyValue);

            When(request => request.TrainingType == TrainingType.Standard, () =>
                {
                    RuleFor(request => request.TrainingCode)
                        .Must(MustBeAcceptableStandardCode)
                        .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode)
                        .WithMessage(ErrorMessages.CreateApprenticeship.InvalidStandardLarsCode);
                });

            When(request => request.TrainingType == TrainingType.Framework, () =>
            {
                RuleFor(request => request.TrainingCode)
                    .Matches(RegexFrameworkCode)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingCode)
                    .WithMessage(ErrorMessages.CreateApprenticeship.InvalidFrameworkLarsCode);
            });
        }

        private static bool MustBeAcceptableStandardCode(string code)
        {
            int validCode;
            if (int.TryParse(code, out validCode))
            {
                return validCode > 0 && validCode <= 9999;
            }
            return false;
        }
    }
}
