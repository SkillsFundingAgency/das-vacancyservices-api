using System.ComponentModel;
using System.Globalization;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using ApprenticeshipVacancy = Esfa.Vacancy.Api.Types.ApprenticeshipVacancy;
using DomainEntities = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public sealed class ApprenticeshipMapper
    {
        private readonly IProvideSettings _provideSettings;
        private readonly AddressMapper _addressMapper = new AddressMapper();
        private const string UnknownText = "Unknown";

        public ApprenticeshipMapper(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public ApprenticeshipVacancy MapToApprenticeshipVacancy(DomainEntities.ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveApprenticeshipVacancyBaseUrlKey);

            var apprenticeship = new ApprenticeshipVacancy
            {
                VacancyReference = apprenticeshipVacancy.VacancyReferenceNumber,
                Title = apprenticeshipVacancy.Title,
                ShortDescription = apprenticeshipVacancy.ShortDescription,
                Description = apprenticeshipVacancy.Description,
                WageUnit = MapWageUnit(apprenticeshipVacancy.WageUnitId),
                WorkingWeek = apprenticeshipVacancy.WorkingWeek,
                WageText = MapWage(apprenticeshipVacancy),
                HoursPerWeek = apprenticeshipVacancy.HoursPerWeek,
                ExpectedDuration = apprenticeshipVacancy.ExpectedDuration,
                ExpectedStartDate = apprenticeshipVacancy.ExpectedStartDate,
                PostedDate = apprenticeshipVacancy.PostedDate,
                ApplicationClosingDate = apprenticeshipVacancy.ApplicationClosingDate,
                InterviewFromDate = apprenticeshipVacancy.InterviewFromDate,
                NumberOfPositions = apprenticeshipVacancy.NumberOfPositions,
                EmployerName = apprenticeshipVacancy.IsAnonymousEmployer ? apprenticeshipVacancy.AnonymousEmployerName : apprenticeshipVacancy.EmployerName,
                EmployerDescription = apprenticeshipVacancy.IsAnonymousEmployer ? apprenticeshipVacancy.AnonymousEmployerDescription : apprenticeshipVacancy.EmployerDescription,
                EmployerWebsite = apprenticeshipVacancy.IsAnonymousEmployer ? null : apprenticeshipVacancy.EmployerWebsite,
                TrainingToBeProvided = apprenticeshipVacancy.TrainingToBeProvided,
                QualificationsRequired = apprenticeshipVacancy.QualificationsRequired,
                SkillsRequired = apprenticeshipVacancy.SkillsRequired,
                PersonalQualities = apprenticeshipVacancy.PersonalQualities,
                ImportantInformation = apprenticeshipVacancy.ImportantInformation,
                FutureProspects = apprenticeshipVacancy.FutureProspects,
                ThingsToConsider = apprenticeshipVacancy.ThingsToConsider,
                LocationType = (VacancyLocationType)apprenticeshipVacancy.VacancyLocationTypeId,
                SupplementaryQuestion1 = apprenticeshipVacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = apprenticeshipVacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{apprenticeshipVacancy.VacancyReferenceNumber}",
                Location = _addressMapper.MapToLocation(apprenticeshipVacancy.Location, showAnonymousEmployerDetails: apprenticeshipVacancy.IsAnonymousEmployer),
                ContactName = apprenticeshipVacancy.ContactName,
                ContactEmail = apprenticeshipVacancy.ContactEmail,
                ContactNumber = apprenticeshipVacancy.ContactNumber,
                TrainingProviderName = apprenticeshipVacancy.TrainingProvider,
                TrainingProviderUkprn = apprenticeshipVacancy.TrainingProviderUkprn,
                TrainingProviderSite = apprenticeshipVacancy.TrainingProviderSite
            };

            MapTrainingDetails(apprenticeshipVacancy, apprenticeship);

            return apprenticeship;
        }

        private WageUnit MapWageUnit(int? wageUnitId)
        {
            if (wageUnitId.HasValue == false)
            {
                return WageUnit.Unspecified;
            }

            switch (wageUnitId.Value)
            {
                case 2:
                    return WageUnit.Weekly;
                case 3:
                    return WageUnit.Monthly;
                case 4:
                    return WageUnit.Annually;
                default:
                    throw new InvalidEnumArgumentException($"Invalid wage unit for a live apprenticeship: {wageUnitId}");
            }
        }

        private string MapWage(DomainEntities.ApprenticeshipVacancy src)
        {
            switch (src.WageType)
            {
                case (int)DomainEntities.WageType.LegacyText:
                    return UnknownText;
                case (int)DomainEntities.WageType.LegacyWeekly:
                case (int)DomainEntities.WageType.Custom:
                    return GetFormattedCurrencyString(src.WeeklyWage) ?? UnknownText;
                case (int)DomainEntities.WageType.ApprenticeshipMinimum:
                    return GetMinimumApprenticeWage(src);
                case (int)DomainEntities.WageType.NationalMinimum:
                    return GetNationalMinimumWageRangeText(src);
                case (int)DomainEntities.WageType.CustomRange:
                    return GetWageRangeText(src);
                case (int)DomainEntities.WageType.CompetitiveSalary:
                    return "Competitive salary";
                case (int)DomainEntities.WageType.ToBeAgreedUponAppointment:
                    return "To be agreed upon appointment";
                case (int)DomainEntities.WageType.Unwaged:
                    return "Unwaged";
                default:
                    return UnknownText;
            }
        }

        private string GetMinimumApprenticeWage(DomainEntities.ApprenticeshipVacancy src)
        {
            return src.MinimumWageRate.HasValue && src.HoursPerWeek.HasValue
                ? GetFormattedCurrencyString(src.MinimumWageRate.Value * src.HoursPerWeek.Value)
                : UnknownText;
        }

        private string GetWageRangeText(DomainEntities.ApprenticeshipVacancy src)
        {
            return $"{GetFormattedCurrencyString(src.WageLowerBound) ?? UnknownText} - {GetFormattedCurrencyString(src.WageUpperBound) ?? UnknownText}";
        }

        private string GetFormattedCurrencyString(decimal? src)
        {
            const string currencyStringFormat = "C";
            return src?.ToString(currencyStringFormat, CultureInfo.GetCultureInfo("en-GB"));
        }

        private string GetNationalMinimumWageRangeText(DomainEntities.ApprenticeshipVacancy src)
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

        private void MapTrainingDetails(DomainEntities.ApprenticeshipVacancy src, ApprenticeshipVacancy dest)
        {
            if (src.Framework != null)
            {
                dest.TrainingType = TrainingType.Framework;
                dest.TrainingTitle = src.Framework.Title;
                dest.TrainingCode = src.Framework.Code.ToString();
            }
            else if (src.Standard != null)
            {
                dest.TrainingType = TrainingType.Standard;
                dest.TrainingTitle = src.Standard.Title;
                dest.TrainingCode = src.Standard.Code.ToString();
            }
            else
            {
                dest.TrainingType = TrainingType.Unavailable;
            }
        }
    }
}