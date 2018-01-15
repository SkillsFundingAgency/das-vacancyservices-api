using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private const string RegexFreeTextWhitelist = @"^[a-zA-Z0-9\u0080-\uFFA7?$@#()""'!,+\-=_:;.&€£*%\s\/\[\]]+$";

        private void WorkingWeekValidator()
        {
            RuleFor(request => request.WorkingWeek)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeekRequired)
                .DependentRules(rules => rules.RuleFor(request => request.WorkingWeek)
                    .MaximumLength(250)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeekLengthGreaterThan250)
                    .Matches(RegexFreeTextWhitelist)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WorkingWeekShouldNotIncludeSpecialCharacters)
                    .WithMessage(string.Format(ErrorMessages.CreateApprenticeship.Whitelist, nameof(CreateApprenticeshipRequest.WorkingWeek))));
        }
    }
}