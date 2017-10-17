using System.Globalization;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Address = Esfa.Vacancy.Api.Types.Address;
using ApprenticeshipVacancy = Esfa.Vacancy.Api.Types.ApprenticeshipVacancy;
using TraineeshipVacancy = Esfa.Vacancy.Api.Types.TraineeshipVacancy;

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

        public ApprenticeshipVacancy MapToApprenticeshipVacancy(Domain.Entities.ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveApprenticeshipVacancyBaseUrlKey);

            var apprenticeship = new ApprenticeshipVacancy
            {
                VacancyReference = apprenticeshipVacancy.VacancyReferenceNumber,
                Title = apprenticeshipVacancy.Title,
                ShortDescription = apprenticeshipVacancy.ShortDescription,
                Description = apprenticeshipVacancy.Description,
                WageUnit = (WageUnit?)apprenticeshipVacancy.WageUnitId,
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
                Location = MapToLocation(apprenticeshipVacancy.Location, showAnonymousEmployerDetails: apprenticeshipVacancy.IsAnonymousEmployer),
                ContactName = apprenticeshipVacancy.ContactName,
                ContactEmail = apprenticeshipVacancy.ContactEmail,
                ContactNumber = apprenticeshipVacancy.ContactNumber
            };

            MapTrainingDetails(apprenticeshipVacancy, apprenticeship);

            return apprenticeship;
        }

        public TraineeshipVacancy MapToTraineeshipVacancy(Domain.Entities.TraineeshipVacancy traineeshipVacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveTraineeshipVacancyBaseUrlKey);

            return new TraineeshipVacancy
            {
                VacancyReference = traineeshipVacancy.VacancyReferenceNumber,
                Title = traineeshipVacancy.Title,
                ShortDescription = traineeshipVacancy.ShortDescription,
                Description = traineeshipVacancy.Description,
                WorkingWeek = traineeshipVacancy.WorkingWeek,
                ExpectedDuration = traineeshipVacancy.ExpectedDuration,
                ExpectedStartDate = traineeshipVacancy.ExpectedStartDate,
                PostedDate = traineeshipVacancy.PostedDate,
                ApplicationClosingDate = traineeshipVacancy.ApplicationClosingDate,
                InterviewFromDate = traineeshipVacancy.InterviewFromDate,
                NumberOfPositions = traineeshipVacancy.NumberOfPositions,
                TraineeshipSector = traineeshipVacancy.TraineeshipSector,
                EmployerName = traineeshipVacancy.IsAnonymousEmployer ? traineeshipVacancy.AnonymousEmployerName : traineeshipVacancy.EmployerName,
                EmployerDescription = traineeshipVacancy.IsAnonymousEmployer ? traineeshipVacancy.AnonymousEmployerDescription : traineeshipVacancy.EmployerDescription,
                EmployerWebsite = traineeshipVacancy.IsAnonymousEmployer ? null : traineeshipVacancy.EmployerWebsite,
                TrainingToBeProvided = traineeshipVacancy.TrainingToBeProvided,
                QualificationsRequired = traineeshipVacancy.QualificationsRequired,
                SkillsRequired = traineeshipVacancy.SkillsRequired,
                PersonalQualities = traineeshipVacancy.PersonalQualities,
                ImportantInformation = traineeshipVacancy.ImportantInformation,
                FutureProspects = traineeshipVacancy.FutureProspects,
                ThingsToConsider = traineeshipVacancy.ThingsToConsider,
                LocationType = (VacancyLocationType)traineeshipVacancy.VacancyLocationTypeId,
                SupplementaryQuestion1 = traineeshipVacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = traineeshipVacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{traineeshipVacancy.VacancyReferenceNumber}",
                Location = MapToLocation(traineeshipVacancy.Location, showAnonymousEmployerDetails: traineeshipVacancy.IsAnonymousEmployer),
                ContactName = traineeshipVacancy.ContactName,
                ContactEmail = traineeshipVacancy.ContactEmail,
                ContactNumber = traineeshipVacancy.ContactNumber
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