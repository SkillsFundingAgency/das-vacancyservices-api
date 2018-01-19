﻿using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureLocationValidator()
        {
            const int addressLineMaxLength = 300;
            const int townMaxLength = 100;

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.AddressLine1)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1IsRequired)
                    .MaximumLength(addressLineMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1MaxLength)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1ShouldNotIncludeSpecialCharacters);
            });

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.AddressLine2)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2IsRequired)
                    .MaximumLength(addressLineMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2MaxLength)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2ShouldNotIncludeSpecialCharacters);
            });

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.AddressLine3)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3IsRequired)
                    .MaximumLength(addressLineMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3MaxLength)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3ShouldNotIncludeSpecialCharacters);
            });

            RuleFor(request => request.AddressLine4)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4MaxLength)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4ShouldNotIncludeSpecialCharacters)
                .When(request => request.LocationType == LocationType.OtherLocation);

            RuleFor(request => request.AddressLine5)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5MaxLength)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5ShouldNotIncludeSpecialCharacters)
                .When(request => request.LocationType == LocationType.OtherLocation);

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.Town)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TownIsRequired)
                    .MaximumLength(townMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TownMaxLength)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.TownShouldNotIncludeSpecialCharacters);
            });

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.Postcode)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.PostcodeIsRequired)
                    .DependentRules(rule => rule.RuleFor(request => request.Postcode)
                        .MustBeAValidPostcode()
                        .WithErrorCode(ErrorCodes.CreateApprenticeship.PostcodeShouldBeValid));
            });
        }
    }
}