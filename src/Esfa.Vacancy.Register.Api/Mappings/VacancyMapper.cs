using System.Globalization;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Address = Esfa.Vacancy.Api.Types.Address;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public sealed class VacancyMapper
    {
        private readonly IProvideSettings _provideSettings;
        private const string UnknownText = "Unknown";

        public VacancyMapper(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public ApprenticeshipVacancy MapToApprenticeshipVacancy(Domain.Entities.Vacancy vacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveApprenticeshipVacancyBaseUrlKey);

            var apprenticeship = new ApprenticeshipVacancy
            {
                VacancyReference = vacancy.VacancyReferenceNumber,
                Title = vacancy.Title,
                ShortDescription = vacancy.ShortDescription,
                Description = vacancy.Description,
                WageUnit = (WageUnit?)vacancy.WageUnitId,
                WorkingWeek = vacancy.WorkingWeek,
                WageText = MapWage(vacancy),
                HoursPerWeek = vacancy.HoursPerWeek,
                ExpectedDuration = vacancy.ExpectedDuration,
                ExpectedStartDate = vacancy.ExpectedStartDate,
                PostedDate = vacancy.PostedDate,
                ApplicationClosingDate = vacancy.ApplicationClosingDate,
                InterviewFromDate = vacancy.InterviewFromDate,
                NumberOfPositions = vacancy.NumberOfPositions,
                EmployerName = vacancy.IsAnonymousEmployer ? vacancy.AnonymousEmployerName : vacancy.EmployerName,
                EmployerDescription = vacancy.IsAnonymousEmployer ? vacancy.AnonymousEmployerDescription : vacancy.EmployerDescription,
                EmployerWebsite = vacancy.IsAnonymousEmployer ? null : vacancy.EmployerWebsite,
                TrainingToBeProvided = vacancy.TrainingToBeProvided,
                QualificationsRequired = vacancy.QualificationsRequired,
                SkillsRequired = vacancy.SkillsRequired,
                PersonalQualities = vacancy.PersonalQualities,
                ImportantInformation = vacancy.ImportantInformation,
                FutureProspects = vacancy.FutureProspects,
                ThingsToConsider = vacancy.ThingsToConsider,
                LocationType = (VacancyLocationType)vacancy.VacancyLocationTypeId,
                SupplementaryQuestion1 = vacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = vacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{vacancy.VacancyReferenceNumber}",
                Location = MapToLocation(vacancy.Location, showAnonymousEmployerDetails: vacancy.IsAnonymousEmployer),
                ContactName = vacancy.ContactName,
                ContactEmail = vacancy.ContactEmail,
                ContactNumber = vacancy.ContactNumber
            };

            MapTrainingDetails(vacancy, apprenticeship);

            return apprenticeship;
        }

        public TraineeshipVacancy MapToTraineeshipVacancy(Domain.Entities.Vacancy vacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveTraineeshipVacancyBaseUrlKey);

            return new TraineeshipVacancy
            {
                VacancyReference = vacancy.VacancyReferenceNumber,
                Title = vacancy.Title,
                ShortDescription = vacancy.ShortDescription,
                Description = vacancy.Description,
                WorkingWeek = vacancy.WorkingWeek,
                ExpectedDuration = vacancy.ExpectedDuration,
                ExpectedStartDate = vacancy.ExpectedStartDate,
                PostedDate = vacancy.PostedDate,
                ApplicationClosingDate = vacancy.ApplicationClosingDate,
                InterviewFromDate = vacancy.InterviewFromDate,
                NumberOfPositions = vacancy.NumberOfPositions,
                TraineeshipSector = vacancy.TraineeshipSector,
                EmployerName = vacancy.IsAnonymousEmployer ? vacancy.AnonymousEmployerName : vacancy.EmployerName,
                EmployerDescription = vacancy.IsAnonymousEmployer ? vacancy.AnonymousEmployerDescription : vacancy.EmployerDescription,
                EmployerWebsite = vacancy.IsAnonymousEmployer ? null : vacancy.EmployerWebsite,
                TrainingToBeProvided = vacancy.TrainingToBeProvided,
                QualificationsRequired = vacancy.QualificationsRequired,
                SkillsRequired = vacancy.SkillsRequired,
                PersonalQualities = vacancy.PersonalQualities,
                ImportantInformation = vacancy.ImportantInformation,
                FutureProspects = vacancy.FutureProspects,
                ThingsToConsider = vacancy.ThingsToConsider,
                LocationType = (VacancyLocationType)vacancy.VacancyLocationTypeId,
                SupplementaryQuestion1 = vacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = vacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{vacancy.VacancyReferenceNumber}",
                Location = MapToLocation(vacancy.Location, showAnonymousEmployerDetails: vacancy.IsAnonymousEmployer),
                ContactName = vacancy.ContactName,
                ContactEmail = vacancy.ContactEmail,
                ContactNumber = vacancy.ContactNumber
            };
        }

        private Address MapToLocation(Domain.Entities.Address location, bool showAnonymousEmployerDetails)
        {
            if (showAnonymousEmployerDetails)
            {
                return new Address
                {
                    Town = location.Town
                };
            }

            return new Address
            {
                AddressLine1 = location.AddressLine1,
                AddressLine2 = location.AddressLine2,
                AddressLine3 = location.AddressLine3,
                AddressLine4 = location.AddressLine4,
                AddressLine5 = location.AddressLine5,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                PostCode = location.PostCode,
                Town = location.Town
            };
        }

        private string MapWage(Domain.Entities.Vacancy src)
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

        private string GetMinimumApprenticeWage(Domain.Entities.Vacancy src)
        {
            return src.MinimumWageRate.HasValue && src.HoursPerWeek.HasValue
                ? GetFormattedCurrencyString(src.MinimumWageRate.Value * src.HoursPerWeek.Value)
                : UnknownText;
        }

        private string GetWageRangeText(Domain.Entities.Vacancy src)
        {
            return $"{GetFormattedCurrencyString(src.WageLowerBound) ?? UnknownText} - {GetFormattedCurrencyString(src.WageUpperBound) ?? UnknownText}";
        }

        private string GetFormattedCurrencyString(decimal? src)
        {
            const string currencyStringFormat = "C";
            return src?.ToString(currencyStringFormat, CultureInfo.GetCultureInfo("en-GB"));
        }

        private string GetNationalMinimumWageRangeText(Domain.Entities.Vacancy src)
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

        private void MapTrainingDetails(Domain.Entities.Vacancy src, ApprenticeshipVacancy dest)
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