using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SearchApprenticeshipVacanciesRequestValidator : AbstractValidator<SearchApprenticeshipVacanciesRequest>
    {
        private readonly ITrainingDetailService _trainingDetailService;
        private const int MinimumPageSize = 1;
        private const int MinimumPageNumber = 1;
        private const int MaximumPageSize = 250;
        private const double MinimumLatitude = -90;
        private const double MaximumLatitude = 90;
        private const double MinimumLongitude = -180;
        private const double MaximumLongitude = 180;
        private const int MinimumDistanceInMiles = 1;
        private const int MaximumDistanceInMiles = 1000;
        private const int UkprnLength = 8;

        public SearchApprenticeshipVacanciesRequestValidator(ITrainingDetailService trainingDetailService)
        {
            _trainingDetailService = trainingDetailService;

            RuleFor(request => request.StandardLarsCodes)
                .NotEmpty()
                .When(request => !request.FrameworkLarsCodes.Any())
                .When(request => !request.NationwideOnly)
                .When(request => !request.PostedInLastNumberOfDays.HasValue)
                .When(request => !request.IsGeoSearch)
                .When(request => !request.Ukprn.HasValue)
                .WithMessage(ErrorMessages.SearchApprenticeships.MinimumRequiredFieldsNotProvided)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.InvalidRequest);

            RuleFor(request => request.NationwideOnly)
                .NotEqual(true)
                .When(request => request.IsGeoSearch)
                .WithMessage(ErrorMessages.SearchApprenticeships.GeoSearchAndNationwideNotAllowed)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.NationwideOnly);

            RuleForEach(request => request.StandardLarsCodes)
                .Must(BeValidNumber)
                .WithMessage((request, value) =>
                    ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Standard, value))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardCode)
                .DependentRules(d => d.RuleForEach(request => request.StandardLarsCodes)
                    .MustAsync(BeAValidStandardCode)
                    .WithMessage((c, value) =>
                        ErrorMessages.SearchApprenticeships.GetTrainingCodeNotFoundErrorMessage(TrainingType.Standard, value))
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.StandardCode));

            RuleForEach(request => request.FrameworkLarsCodes)
                .Must(BeValidNumber)
                .WithMessage((request, value) => ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, value))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.FrameworkCode)
                .DependentRules(d => d.RuleForEach(request => request.FrameworkLarsCodes)
                    .MustAsync(BeAValidFrameworkCode)
                    .WithMessage((c, value) =>
                        ErrorMessages.SearchApprenticeships.GetTrainingCodeNotFoundErrorMessage(TrainingType.Framework, value))
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.FrameworkCode));

            RuleFor(r => r.PageSize)
                .InclusiveBetween(MinimumPageSize, MaximumPageSize)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageSize);

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(MinimumPageNumber)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PageNumber);

            RuleFor(r => r.PostedInLastNumberOfDays)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.PostedInLastNumberOfDays);

            RuleFor(request => request.Latitude)
                .NotNull()
                .When(request => request.IsGeoSearch)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.Latitude)
                .WithMessage(ErrorMessages.SearchApprenticeships.GetGeoLocationFieldNotProvidedErrorMessage(nameof(SearchApprenticeshipVacanciesRequest.Latitude)))
                .DependentRules(rules => rules.RuleFor(request => request.Latitude)
                    .InclusiveBetween(MinimumLatitude, MaximumLatitude)
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.Latitude));

            RuleFor(request => request.Longitude)
                .NotNull()
                .When(request => request.IsGeoSearch)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.Longitude)
                .WithMessage(ErrorMessages.SearchApprenticeships.GetGeoLocationFieldNotProvidedErrorMessage(nameof(SearchApprenticeshipVacanciesRequest.Longitude)))
                .DependentRules(rules => rules.RuleFor(request => request.Longitude)
                    .InclusiveBetween(MinimumLongitude, MaximumLongitude)
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.Longitude));

            RuleFor(request => request.DistanceInMiles)
                .NotNull()
                .When(request => request.IsGeoSearch)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.DistanceInMiles)
                .WithMessage(ErrorMessages.SearchApprenticeships.GetGeoLocationFieldNotProvidedErrorMessage(nameof(SearchApprenticeshipVacanciesRequest.DistanceInMiles)))
                .DependentRules(rules => rules.RuleFor(request => request.DistanceInMiles)
                    .InclusiveBetween(MinimumDistanceInMiles, MaximumDistanceInMiles)
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.DistanceInMiles));

            RuleFor(request => request.SortBy)
                .Must(BeValidSortBy)
                .When(request => !string.IsNullOrEmpty(request.SortBy))
                .WithErrorCode(ErrorCodes.SearchApprenticeships.SortBy)
                .WithMessage(request => ErrorMessages.SearchApprenticeships.SortByValueNotAllowed(request.SortBy))
                .DependentRules(rules => rules.RuleFor(request => request.SortBy)
                    .NotEqual(SortBy.Distance.ToString(), StringComparer.InvariantCultureIgnoreCase)
                    .When(request => !request.IsGeoSearch)
                    .WithErrorCode(ErrorCodes.SearchApprenticeships.SortBy)
                    .WithMessage(ErrorMessages.SearchApprenticeships.SortByDistanceOnlyWhenGeoSearch));

            RuleFor(request => request.Ukprn)
                .Must(ukprn => ukprn.ToString().Length == UkprnLength)
                .When(request => request.Ukprn.HasValue)
                .WithErrorCode(ErrorCodes.SearchApprenticeships.Ukprn)
                .WithMessage(ErrorMessages.SearchApprenticeships.UkprnIsInvalid);
        }

        private static bool BeValidSortBy(string value)
        {
            SortBy sortBy;
            return Enum.TryParse(value, true, out sortBy);
        }

        private static bool BeValidNumber(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        private async Task<bool> BeAValidFrameworkCode(string frameworkCode, CancellationToken token)
        {
            IEnumerable<TrainingDetail> frameworks = await _trainingDetailService.GetAllFrameworkDetailsAsync()
                                                                                 .ConfigureAwait(false);
            return frameworks.Any(detail => IsValidFrameworkCode(detail, frameworkCode));
        }

        private static bool IsValidFrameworkCode(TrainingDetail framework, string frameworkCode)
        {
            var code = framework.TrainingCode.Split('-').FirstOrDefault();
            return !String.IsNullOrEmpty(code) && code.Equals(frameworkCode) && !framework.HasExpired;
        }

        private async Task<bool> BeAValidStandardCode(string standardCode, CancellationToken token)
        {
            IEnumerable<TrainingDetail> standards = await _trainingDetailService.GetAllStandardDetailsAsync()
                                                                                .ConfigureAwait(false);
            return standards.Any(standard =>
                standard.TrainingCode.Equals(standardCode.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                !standard.HasExpired);
        }
    }
}
