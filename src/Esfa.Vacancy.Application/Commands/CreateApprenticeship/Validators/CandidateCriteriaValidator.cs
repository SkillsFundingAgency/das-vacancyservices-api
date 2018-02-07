namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    using Domain.Validation;
    using FluentValidation;

    public partial class CreateApprenticeshipRequestValidator
    {
        private const int DesiredSkillsMaximumLength = 4000;

        private void ConfigureCandidateCriteriaValidator()
        {
            RuleFor(request => request.DesiredSkills)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredSkills)
                .DependentRules(rules => rules.RuleFor(request => request.DesiredSkills)
                                              .MaximumLength(DesiredSkillsMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredSkills)
                                              .MatchesAllowedFreeTextCharacters()
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredSkills)
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.DesiredSkills,
                                                  ErrorCodes.CreateApprenticeship.DesiredSkills)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredSkills));

            RuleFor(request => request.DesiredPersonalQualities)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                .DependentRules(rules => rules.RuleFor(request => request.DesiredPersonalQualities)
                                              .MaximumLength(DesiredSkillsMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                                              .MatchesAllowedFreeTextCharacters()
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities,
                                                  ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities));

        }
    }
}
