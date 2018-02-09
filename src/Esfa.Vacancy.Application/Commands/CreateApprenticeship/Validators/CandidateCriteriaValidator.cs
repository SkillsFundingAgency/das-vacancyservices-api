using System;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
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
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.DesiredSkills)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredSkills));

            RuleFor(request => request.DesiredPersonalQualities)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                .DependentRules(rules => rules.RuleFor(request => request.DesiredPersonalQualities)
                                              .MaximumLength(CandidateCriteriaMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities)
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship
                                                                                              .DesiredPersonalQualities)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredPersonalQualities));

            RuleFor(request => request.DesiredQualifications)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications)
                .DependentRules(rules => rules.RuleFor(request => request.DesiredQualifications)
                                              .MaximumLength(CandidateCriteriaMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications)
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes
                                                                                    .CreateApprenticeship.DesiredQualifications)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.DesiredQualifications));

            RuleFor(request => request.FutureProspects)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.FutureProspects)
                .DependentRules(rules => rules.RuleFor(request => request.FutureProspects)
                                              .MaximumLength(CandidateCriteriaMaximumLength)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.FutureProspects)
                                              .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes
                                                                                    .CreateApprenticeship.FutureProspects)
                                              .WithErrorCode(ErrorCodes.CreateApprenticeship.FutureProspects));

            When(request => !String.IsNullOrEmpty(request.ThingsToConsider), () =>
            {
                RuleFor(request => request.ThingsToConsider)
                    .MaximumLength(CandidateCriteriaMaximumLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ThingsToConsider)
                    .MatchesAllowedHtmlFreeTextCharacters(ErrorCodes.CreateApprenticeship.ThingsToConsider)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ThingsToConsider);
            });
        }
    }
}
