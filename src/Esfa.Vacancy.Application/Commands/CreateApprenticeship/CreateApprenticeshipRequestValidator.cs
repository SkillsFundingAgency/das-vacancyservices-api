using System;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;


namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        private const string TitleApprentice = "apprentice";
        private const int TitleMaximumLength = 100;
        public CreateApprenticeshipRequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .WithErrorCode(TitleIsRequired)
                .DependentRules(rules =>
                {
                    rules.RuleFor(request => request.Title)
                        .MaximumLength(TitleMaximumLength)
                        .WithErrorCode(TitleMaximumFieldLength)
                        .Must(title => title.IndexOf(TitleApprentice, StringComparison.OrdinalIgnoreCase) >= 0)
                        .WithErrorCode(TitleShouldIncludeWordApprentice);
                });
        }
    }
}