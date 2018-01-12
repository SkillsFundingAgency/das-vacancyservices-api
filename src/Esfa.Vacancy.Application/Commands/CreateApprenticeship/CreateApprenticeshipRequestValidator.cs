using System;
using System.Linq.Expressions;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        public CreateApprenticeshipRequestValidator()
        {
            SetValidatorDisallowEmpty(request => request.Title, TitleIsRequired, new TitleValidator());
            SetValidatorDisallowEmpty(request => request.ShortDescription, ShortDescriptionIsRequired, new ShortDescriptionValidator());
            SetValidatorDisallowEmpty(request => request.LongDescription, LongDescriptionIsRequired, new LongDescriptionValidator());
            SetValidatorDisallowEmpty(request => request.ApplicationClosingDate, ApplicationClosingDateRequired, new ApplicationClosingDateValidator());

            RuleFor(request => request.ExpectedStartDate)
                .NotEmpty()
                .WithErrorCode(ExpectedStartDateRequired)
                .DependentRules(rules => rules.RuleFor(request => request.ExpectedStartDate)
                    .GreaterThanOrEqualTo(request => request.ApplicationClosingDate.Date.AddDays(1))
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.ExpectedStartDateBeforeClosingDate)
                    .WithMessage(ErrorMessages.CreateApprenticeship.ExpectedStartDateBeforeClosingDate));

            WorkingWeekValidator();
            HoursPerWeekValidator();
            ValidateLocationType();
        }

        private void SetValidatorDisallowEmpty<TProperty>(Expression<Func<CreateApprenticeshipRequest, TProperty>> selector, string emptyErrorCode, AbstractValidator<TProperty> validatorToAdd)
        {
            //AbstractValidators do not work against null fields so adding the validator as a dependent rule and doing a NotEmpty check first
            RuleFor(selector)
                .NotEmpty()
                .WithErrorCode(emptyErrorCode)
                .DependentRules(rules =>
                {
                    rules.RuleFor(selector).SetValidator(validatorToAdd);
                });
        }

        private void ValidateLocationType()
        {
            RuleFor(request => request.LocationType)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.LocationTypeIsRequired);
        }
    }
}