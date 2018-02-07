namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    using Domain.Validation;
    using FluentValidation;

    public partial class CreateApprenticeshipRequestValidator
    {
        private const int CandidateCriteriaMaximumLength = 4000;

        private void ConfigureCandidateCriteriaValidator()
        {
            RuleFor(request => request.DesiredSkills)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredSkills)
                .DependentRules(rules => rules.RuleFor(request => request.DesiredSkills)
                                              .MaximumLength(CandidateCriteriaMaximumLength)
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
                                              .MaximumLength(CandidateCriteriaMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                                              .MatchesAllowedFreeTextCharacters()
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities,
                                                  ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities));

            RuleFor(request => request.DesiredQualifications)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications)
                .DependentRules(rules => rules.RuleFor(request => request.DesiredQualifications)
                                              .MaximumLength(CandidateCriteriaMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications)
                                              .MatchesAllowedFreeTextCharacters()
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications)
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.DesiredQualifications,
                                                  ErrorCodes.CreateApprenticeship.DesiredQualifications)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications));
        }
    }
}
