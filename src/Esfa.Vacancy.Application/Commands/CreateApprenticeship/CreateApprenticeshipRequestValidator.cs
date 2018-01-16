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
            ValidateLocation();
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

        private void ValidateLocation()
        {
            const int addressLineMaxLength = 300;
            const int townMaxLength = 100;

            RuleFor(request => request.AddressLine1)
                .NotEmpty()
                .When(request => request.LocationType == LocationType.OtherLocation)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1IsRequired)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1MaxLength)
                .MustContainAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1ShouldNotIncludeSpecialCharacters);

            RuleFor(request => request.AddressLine2)
                .NotEmpty()
                .When(request => request.LocationType == LocationType.OtherLocation)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2IsRequired)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2MaxLength)
                .MustContainAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2ShouldNotIncludeSpecialCharacters);

            RuleFor(request => request.AddressLine3)
                .NotEmpty()
                .When(request => request.LocationType == LocationType.OtherLocation)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3IsRequired)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3MaxLength)
                .MustContainAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3ShouldNotIncludeSpecialCharacters);

            RuleFor(request => request.AddressLine4)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4MaxLength)
                .MustContainAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4ShouldNotIncludeSpecialCharacters)
                .When(request => request.LocationType == LocationType.OtherLocation);

            RuleFor(request => request.AddressLine5)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5MaxLength)
                .MustContainAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5ShouldNotIncludeSpecialCharacters)
                .When(request => request.LocationType == LocationType.OtherLocation);

            RuleFor(request => request.Town)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TownIsRequired)
                .MaximumLength(townMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TownMaxLength)
                .MustContainAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.TownShouldNotIncludeSpecialCharacters)
                .When(request => request.LocationType == LocationType.OtherLocation);

            RuleFor(request => request.PostCode)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.PostcodeIsRequired)
                .MustBeAValidPostcode()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.PostcodeShouldBeValid)
                .When(request => request.LocationType == LocationType.OtherLocation);

        }
    }
}