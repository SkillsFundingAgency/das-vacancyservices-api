using System;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        private const string TitleApprentice = "apprentice";
        public CreateApprenticeshipRequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .DependentRules(rules =>
                {
                    rules.RuleFor(request => request.Title)
                        .Must(title => title.IndexOf(TitleApprentice, StringComparison.OrdinalIgnoreCase) >= 0);
                });
        }
    }
}