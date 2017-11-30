using System.ComponentModel;
using System.Globalization;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Settings;
using ApprenticeshipVacancy = Esfa.Vacancy.Api.Types.ApprenticeshipVacancy;
using TrainingType = Esfa.Vacancy.Api.Types.TrainingType;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public sealed class ApprenticeshipMapper : IApprenticeshipMapper
    {
        private readonly IProvideSettings _provideSettings;
        private readonly AddressMapper _addressMapper = new AddressMapper();
        private const string UnknownText = "Unknown";
        private const int Nationwide = 3;

        public ApprenticeshipMapper(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public ApprenticeshipVacancy MapToApprenticeshipVacancy(Domain.Entities.ApprenticeshipVacancy apprenticeshipVacancy)
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
                IsNationwide = apprenticeshipVacancy.VacancyLocationTypeId == Nationwide,
                SupplementaryQuestion1 = apprenticeshipVacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = apprenticeshipVacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{apprenticeshipVacancy.VacancyReferenceNumber}",
                Location = _addressMapper.MapToLocation(apprenticeshipVacancy.Location, showAnonymousEmployerDetails: apprenticeshipVacancy.IsAnonymousEmployer),
                ContactName = apprenticeshipVacancy.ContactName,
                ContactEmail = apprenticeshipVacancy.ContactEmail,
                ContactNumber = apprenticeshipVacancy.ContactNumber,
                TrainingProviderName = apprenticeshipVacancy.TrainingProvider,
                TrainingProviderUkprn = apprenticeshipVacancy.TrainingProviderUkprn,
                TrainingProviderSite = apprenticeshipVacancy.TrainingProviderSite,
                IsEmployerDisabilityConfident = apprenticeshipVacancy.IsDisabilityConfident
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

        private string MapWage(Domain.Entities.ApprenticeshipVacancy src)
        {
            switch (src.WageType)
            {
                case (int)WageType.LegacyText:
                    return UnknownText;
                case (int)WageType.LegacyWeekly:
                case (int)WageType.Custom:
                    return GetFormattedCurrencyString(src.WeeklyWage) ?? UnknownText;
                case (int)WageType.ApprenticeshipMinimum:
                    return GetMinimumApprenticeWage(src);
                case (int)WageType.NationalMinimum:
                    return GetNationalMinimumWageRangeText(src);
                case (int)WageType.CustomRange:
                    return GetWageRangeText(src);
                case (int)WageType.CompetitiveSalary:
                    return "Competitive salary";
                case (int)WageType.ToBeAgreedUponAppointment:
                    return "To be agreed upon appointment";
                case (int)WageType.Unwaged:
                    return "Unwaged";
                default:
                    return UnknownText;
            }
        }

        private string GetMinimumApprenticeWage(Domain.Entities.ApprenticeshipVacancy src)
        {
            return src.MinimumWageRate.HasValue && src.HoursPerWeek.HasValue
                ? GetFormattedCurrencyString(src.MinimumWageRate.Value * src.HoursPerWeek.Value)
                : UnknownText;
        }

        private string GetWageRangeText(Domain.Entities.ApprenticeshipVacancy src)
        {
            return $"{GetFormattedCurrencyString(src.WageLowerBound) ?? UnknownText} - {GetFormattedCurrencyString(src.WageUpperBound) ?? UnknownText}";
        }

        private string GetFormattedCurrencyString(decimal? src)
        {
            const string currencyStringFormat = "C";
            return src?.ToString(currencyStringFormat, CultureInfo.GetCultureInfo("en-GB"));
        }

        private string GetNationalMinimumWageRangeText(Domain.Entities.ApprenticeshipVacancy src)
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

        private void MapTrainingDetails(Domain.Entities.ApprenticeshipVacancy src, ApprenticeshipVacancy dest)
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