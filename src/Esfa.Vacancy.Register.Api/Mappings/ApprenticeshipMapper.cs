using System.ComponentModel;
using System.Globalization;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using ApiTypes = Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public sealed class ApprenticeshipMapper : IApprenticeshipMapper
    {
        private readonly IProvideSettings _provideSettings;
        private readonly GeoCodedAddressMapper _geoCodedAddressMapper = new GeoCodedAddressMapper();
        private const string UnknownText = "Unknown";
        private const int Nationwide = 3;

        public ApprenticeshipMapper(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public ApiTypes.ApprenticeshipVacancy MapToApprenticeshipVacancy(ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeys.LiveApprenticeshipVacancyBaseUrlKey);

            var apprenticeship = new ApiTypes.ApprenticeshipVacancy
            {
                VacancyReference = apprenticeshipVacancy.VacancyReferenceNumber,
                Title = apprenticeshipVacancy.Title,
                ShortDescription = apprenticeshipVacancy.ShortDescription,
                Description = apprenticeshipVacancy.Description,
                WageUnit = MapWageUnit(apprenticeshipVacancy.WageUnitId),
                WorkingWeek = apprenticeshipVacancy.WorkingWeek,
                WageText = MapWage(apprenticeshipVacancy),
                WageAdditionalInformation = null,
                HoursPerWeek = apprenticeshipVacancy.HoursPerWeek,
                ExpectedDuration = apprenticeshipVacancy.ExpectedDuration,
                ExpectedStartDate = apprenticeshipVacancy.ExpectedStartDate,
                PostedDate = apprenticeshipVacancy.PostedDate,
                ApplicationClosingDate = apprenticeshipVacancy.ApplicationClosingDate,
                InterviewFromDate = apprenticeshipVacancy.InterviewFromDate, //TODO remove
                NumberOfPositions = apprenticeshipVacancy.NumberOfPositions,
                EmployerName = apprenticeshipVacancy.IsAnonymousEmployer ? apprenticeshipVacancy.AnonymousEmployerName : apprenticeshipVacancy.EmployerName,
                EmployerDescription = apprenticeshipVacancy.IsAnonymousEmployer ? apprenticeshipVacancy.AnonymousEmployerDescription : apprenticeshipVacancy.EmployerDescription,
                EmployerWebsite = apprenticeshipVacancy.IsAnonymousEmployer ? null : apprenticeshipVacancy.EmployerWebsite,
                TrainingToBeProvided = apprenticeshipVacancy.TrainingToBeProvided,
                QualificationsRequired = apprenticeshipVacancy.QualificationsRequired,
                SkillsRequired = apprenticeshipVacancy.SkillsRequired,
                PersonalQualities = apprenticeshipVacancy.PersonalQualities,
                ImportantInformation = apprenticeshipVacancy.ImportantInformation, //TODO check if can be removed
                FutureProspects = apprenticeshipVacancy.FutureProspects,
                ThingsToConsider = apprenticeshipVacancy.ThingsToConsider,
                IsNationwide = apprenticeshipVacancy.VacancyLocationTypeId == Nationwide,
                SupplementaryQuestion1 = apprenticeshipVacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = apprenticeshipVacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{apprenticeshipVacancy.VacancyReferenceNumber}",
                Location = _geoCodedAddressMapper.MapToLocation(apprenticeshipVacancy.Location, showAnonymousEmployerDetails: apprenticeshipVacancy.IsAnonymousEmployer),
                ContactName = apprenticeshipVacancy.ContactName,
                ContactEmail = apprenticeshipVacancy.ContactEmail,
                ContactNumber = apprenticeshipVacancy.ContactNumber,
                TrainingProviderName = apprenticeshipVacancy.TrainingProvider,
                TrainingProviderUkprn = apprenticeshipVacancy.TrainingProviderUkprn,
                TrainingProviderSite = apprenticeshipVacancy.TrainingProviderSite, //This is mapped to provider's trading name
                IsEmployerDisabilityConfident = apprenticeshipVacancy.IsDisabilityConfident,
                ApprenticeshipLevel = MapEducationLevel(apprenticeshipVacancy.ApprenticeshipTypeId)
            };

            MapTrainingDetails(apprenticeshipVacancy, apprenticeship);

            return apprenticeship;
        }

        private string MapEducationLevel(int apprenticeshipTypeId)
        {
            var educationLevel = (ApiTypes.EducationLevel)apprenticeshipTypeId;
            return educationLevel.ToString();
        }

        private ApiTypes.WageUnit MapWageUnit(int? wageUnitId)
        {
            if (wageUnitId.HasValue == false)
            {
                return ApiTypes.WageUnit.Unspecified;
            }

            switch (wageUnitId.Value)
            {
                case 2:
                    return ApiTypes.WageUnit.Weekly;
                case 3:
                    return ApiTypes.WageUnit.Monthly;
                case 4:
                    return ApiTypes.WageUnit.Annually;
                default:
                    throw new InvalidEnumArgumentException($"Invalid wage unit for a live apprenticeship: {wageUnitId}");
            }
        }

        private string MapWage(ApprenticeshipVacancy src)
        {
            switch (src.WageType)
            {
                case (int)LegacyWageType.LegacyText:
                    return UnknownText;
                case (int)LegacyWageType.LegacyWeekly:
                case (int)LegacyWageType.Custom:
                    return GetFormattedCurrencyString(src.WeeklyWage) ?? UnknownText;
                case (int)LegacyWageType.ApprenticeshipMinimum:
                    return GetMinimumApprenticeWage(src);
                case (int)LegacyWageType.NationalMinimum:
                    return GetNationalMinimumWageRangeText(src);
                case (int)LegacyWageType.CustomRange:
                    return GetWageRangeText(src);
                case (int)LegacyWageType.CompetitiveSalary:
                    return "Competitive salary";
                case (int)LegacyWageType.ToBeAgreedUponAppointment:
                    return "To be agreed upon appointment";
                case (int)LegacyWageType.Unwaged:
                    return "Unwaged";
                default:
                    return UnknownText;
            }
        }

        private string GetMinimumApprenticeWage(ApprenticeshipVacancy src)
        {
            return src.MinimumWageRate.HasValue && src.HoursPerWeek.HasValue
                ? GetFormattedCurrencyString(src.MinimumWageRate.Value * src.HoursPerWeek.Value)
                : UnknownText;
        }

        private string GetWageRangeText(ApprenticeshipVacancy src)
        {
            return $"{GetFormattedCurrencyString(src.WageLowerBound) ?? UnknownText} - {GetFormattedCurrencyString(src.WageUpperBound) ?? UnknownText}";
        }

        private string GetFormattedCurrencyString(decimal? src)
        {
            const string currencyStringFormat = "C";
            return src?.ToString(currencyStringFormat, CultureInfo.GetCultureInfo("en-GB"));
        }

        private string GetNationalMinimumWageRangeText(ApprenticeshipVacancy src)
        {
            if (!src.HoursPerWeek.HasValue || src.HoursPerWeek <= 0)
            {
                return UnknownText;
            }

            if (!src.MinimumWageLowerBound.HasValue && !src.MinimumWageUpperBound.HasValue)
            {
                return UnknownText;
            }

            var lowerMinimumLimit = src.MinimumWageLowerBound * src.HoursPerWeek;
            var upperMinimumLimit = src.MinimumWageUpperBound * src.HoursPerWeek;

            var minLowerBoundSection = GetFormattedCurrencyString(lowerMinimumLimit) ?? UnknownText;
            var minUpperBoundSection = GetFormattedCurrencyString(upperMinimumLimit) ?? UnknownText;

            return $"{minLowerBoundSection} - {minUpperBoundSection}";
        }

        private void MapTrainingDetails(ApprenticeshipVacancy src, ApiTypes.ApprenticeshipVacancy dest)
        {
            if (src.Framework != null)
            {
                dest.TrainingType = ApiTypes.TrainingType.Framework;
                dest.TrainingTitle = src.Framework.Title;
                dest.TrainingCode = src.Framework.Code.ToString();
            }
            else if (src.Standard != null)
            {
                dest.TrainingType = ApiTypes.TrainingType.Standard;
                dest.TrainingTitle = src.Standard.Title;
                dest.TrainingCode = src.Standard.Code.ToString();
            }
            else
            {
                dest.TrainingType = ApiTypes.TrainingType.Unavailable;
            }
        }
    }
}