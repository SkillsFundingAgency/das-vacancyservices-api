﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequestValidator : AbstractValidator<SearchApprenticeshipVacanciesRequest>
    {
        private readonly IFrameworkCodeRepository _frameworkCodeRepository;
        private readonly IStandardRepository _standardRepository;
        private const int MinimumPageSize = 1;
        private const int MinimumPageNumber = 1;
        private const int MaximumPageSize = 250;
        private const double MinimumLatitude = -90;
        private const double MaximumLatitude = 90;
        private const double MinimumLongitude = -180;
        private const double MaximumLongitude = 180;
        private const int MinimumDistanceInMiles = 1;
        private const int MaximumDistanceInMiles = 1000;

        public SearchApprenticeshipVacanciesRequestValidator(
            IFrameworkCodeRepository frameworkCodeRepository,
            IStandardRepository standardRepository)
        {
            _frameworkCodeRepository = frameworkCodeRepository;
            _standardRepository = standardRepository;

            RuleFor(request => request.StandardLarsCodes)
                .NotEmpty()
                .When(request => !request.FrameworkLarsCodes.Any())
                .When(request => !request.NationwideOnly)
                .When(request => !request.PostedInLastNumberOfDays.HasValue)
                .When(request => !request.IsGeoSearch)
                .WithMessage(ErrorMessages.SearchApprenticeships.MinimumRequiredFieldsNotProvided)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.MinimumRequiredFieldsNotProvided);

            RuleFor(request => request.NationwideOnly)
                .NotEqual(true)
                .When(request => request.IsGeoSearch)
                .WithMessage(ErrorMessages.SearchApprenticeships.GeoSearchAndNationwideNotAllowed)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.GeoSearchAndNationwideNotAllowed);

            RuleForEach(request => request.StandardLarsCodes)
                .Must(BeValidNumber)
                .WithMessage((request, value) =>
                    ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Standard, value))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardCodeNotInt32)
                .DependentRules(d => d.RuleForEach(request => request.StandardLarsCodes)
                    .MustAsync(BeAValidStandardCode)
                    .WithMessage((c, value) =>
                        ErrorMessages.SearchApprenticeships.GetTrainingCodeNotFoundErrorMessage(TrainingType.Standard, value))
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardCodeNotFound));

            RuleForEach(request => request.FrameworkLarsCodes)
                .Must(BeValidNumber)
                .WithMessage((request, value) => ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, value))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .DependentRules(d => d.RuleForEach(request => request.FrameworkLarsCodes)
                    .MustAsync(BeAValidFrameworkCode)
                    .WithMessage((c, value) =>
                        ErrorMessages.SearchApprenticeships.GetTrainingCodeNotFoundErrorMessage(TrainingType.Framework, value))
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.FrameworkCodeNotFound));

            RuleFor(r => r.PageSize)
                .InclusiveBetween(MinimumPageSize, MaximumPageSize)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageSizeOutsideRange);

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(MinimumPageNumber)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageNumberLessThan1);

            RuleFor(r => r.PostedInLastNumberOfDays)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PostedInLastNumberOfDaysLessThan0);

            RuleFor(request => request.Latitude)
                .NotNull()
                .When(request => request.IsGeoSearch)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.LatitudeMissingFromGeoSearch)
                .WithMessage(ErrorMessages.SearchApprenticeships.GetGeoLocationFieldNotProvidedErrorMessage(nameof(SearchApprenticeshipVacanciesRequest.Latitude)))
                .InclusiveBetween(MinimumLatitude, MaximumLatitude)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.LatitudeOutsideRange);

            RuleFor(request => request.Longitude)
                .NotNull()
                .When(request => request.IsGeoSearch)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.LongitudeMissingFromGeoSearch)
                .WithMessage(ErrorMessages.SearchApprenticeships.GetGeoLocationFieldNotProvidedErrorMessage(nameof(SearchApprenticeshipVacanciesRequest.Longitude)))
                .InclusiveBetween(MinimumLongitude, MaximumLongitude)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.LongitudeOutsideRange);

            RuleFor(request => request.DistanceInMiles)
                .NotNull()
                .When(request => request.IsGeoSearch)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.DistanceMissingFromGeoSearch)
                .WithMessage(ErrorMessages.SearchApprenticeships.GetGeoLocationFieldNotProvidedErrorMessage(nameof(SearchApprenticeshipVacanciesRequest.DistanceInMiles)))
                .InclusiveBetween(MinimumDistanceInMiles, MaximumDistanceInMiles)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.DistanceOutsideRange);

            RuleFor(request => request.SortBy)
                .IsInEnum()
                .When(request => request.SortBy.HasValue)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.InvalidSortBy)
                .NotEqual(SortBy.Distance)
                .When(request => !request.IsGeoSearch)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.SortByDistanceOnlyWhenGeoSearch)
                .WithMessage(ErrorMessages.SearchApprenticeships.SortByDistanceOnlyWhenGeoSearch);
        }

        private static bool BeValidNumber(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        private async Task<bool> BeAValidFrameworkCode(string frameworkCode, CancellationToken token)
        {
            var validFrameworks = await _frameworkCodeRepository.GetAsync();

            return validFrameworks.Any(larsCode =>
                larsCode.Equals(frameworkCode.Trim(), StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<bool> BeAValidStandardCode(string standardCode, CancellationToken token)
        {
            var validStandards = await _standardRepository.GetStandardIdsAsync();

            return validStandards.Any(larsCode =>
                larsCode.Equals(int.Parse(standardCode)));
        }
    }
}
