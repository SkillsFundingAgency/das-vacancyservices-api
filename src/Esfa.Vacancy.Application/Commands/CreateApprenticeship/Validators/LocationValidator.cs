﻿using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public partial class CreateApprenticeshipRequestValidator
    {
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