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
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1)
                    .MaximumLength(addressLineMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine1);
            });

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.AddressLine2)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2)
                    .MaximumLength(addressLineMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine2);
            });

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.AddressLine3)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3)
                    .MaximumLength(addressLineMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine3);
            });

            RuleFor(request => request.AddressLine4)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine4)
                .When(request => request.LocationType == LocationType.OtherLocation);

            RuleFor(request => request.AddressLine5)
                .MaximumLength(addressLineMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5)
                .When(request => request.LocationType == LocationType.OtherLocation);

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.Town)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.Town)
                    .MaximumLength(townMaxLength)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.Town)
                    .MatchesAllowedFreeTextCharacters()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.Town);
            });

            When(request => request.LocationType == LocationType.OtherLocation, () =>
            {
                RuleFor(request => request.Postcode)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.Postcode)
                    .DependentRules(rule => rule.RuleFor(request => request.Postcode)
                        .MustBeAValidPostcode()
                        .WithErrorCode(ErrorCodes.CreateApprenticeship.Postcode));
            });
        }
    }
}