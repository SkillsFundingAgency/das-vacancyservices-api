using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const int TrainingToBeProvidedMaximumLength = 4000;

        private void ConfigureTrainingToBeProvided()
        {
            RuleFor(request => request.TrainingToBeProvided)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingToBeProvided)
                .DependentRules(rules => rules.RuleFor(request => request.TrainingToBeProvided)
                                              .MaximumLength(TrainingToBeProvidedMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingToBeProvided)
                                              .MatchesAllowedHtmlFreeTextCharacters(
                                                  ErrorCodes.CreateApprenticeship.TrainingToBeProvided,
                                                  ErrorCodes.CreateApprenticeship.TrainingToBeProvided)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.TrainingToBeProvided));
        }
    }
}
