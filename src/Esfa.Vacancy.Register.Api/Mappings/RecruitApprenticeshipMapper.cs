using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using ApiTypes = Esfa.Vacancy.Api.Types;
using recruitEntities = SFA.DAS.Recruit.Vacancies.Client.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class RecruitApprenticeshipMapper : IRecruitVacancyMapper
    {
        public const string FixedWageType = "FixedWage";
        public const string NationalMinimumWageForApprenticesWageType = "NationalMinimumWageForApprentices";
        public const string NationalMinimumWageWageType = "NationalMinimumWage";
        public const string UnspecifiedWageType = "Unspecified";
        public const string UnknownText = "Unknown";

        private readonly IProvideSettings _provideSettings;
        private readonly ITrainingDetailService _trainingDetailService;
        private readonly IGetMinimumWagesService _minimumWagesService;

        public RecruitApprenticeshipMapper(IProvideSettings provideSettings,
            ITrainingDetailService trainingDetailService, IGetMinimumWagesService minimumWagesService)
        {
            _provideSettings = provideSettings;
            _trainingDetailService = trainingDetailService;
            _minimumWagesService = minimumWagesService;
        }

        public async Task<ApiTypes.ApprenticeshipVacancy> MapFromRecruitVacancy(recruitEntities.LiveVacancy liveVacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeys.LiveApprenticeshipVacancyBaseUrlKey);
            
            var trainingType =
                (ApiTypes.TrainingType)Enum.Parse(typeof(ApiTypes.TrainingType), liveVacancy.ProgrammeType);

            var trainingCode = GetTrainingCode(trainingType, liveVacancy.ProgrammeId);

            var trainingTitle = await GetTrainingTitle(trainingType, trainingCode);

            var qualifications = GetVacancyQualification(liveVacancy);

            var skills = string.Join(",", liveVacancy.Skills);

            var wageText = await GetWageText(liveVacancy.Wage, liveVacancy.StartDate);

            var duration = GetDurationAsText(liveVacancy.Wage);

            var wageUnit = GetWageUnit(liveVacancy.Wage.WageType);

            var apprenticeship = new ApiTypes.ApprenticeshipVacancy
            {
                VacancyReference = liveVacancy.VacancyReference,
                Title = liveVacancy.Title,
                ShortDescription = liveVacancy.ShortDescription,
                Description = liveVacancy.Description,
                WageUnit = wageUnit,
                WorkingWeek = liveVacancy.Wage.WorkingWeekDescription,
                WageText = wageText,
                WageAdditionalInformation = liveVacancy.Wage.WageAdditionalInformation,
                HoursPerWeek = liveVacancy.Wage.WeeklyHours,
                ExpectedDuration = duration,
                ExpectedStartDate = liveVacancy.StartDate,
                PostedDate = liveVacancy.LiveDate,
                ApplicationClosingDate = liveVacancy.ClosingDate,
                NumberOfPositions = liveVacancy.NumberOfPositions,
                EmployerName = liveVacancy.EmployerName,
                EmployerDescription = liveVacancy.EmployerDescription,
                EmployerWebsite = liveVacancy.EmployerWebsiteUrl,
                ContactName = liveVacancy.EmployerContactName,
                ContactEmail = liveVacancy.EmployerContactEmail,
                ContactNumber = liveVacancy.EmployerContactPhone,
                TrainingToBeProvided = liveVacancy.TrainingDescription,
                QualificationsRequired = qualifications,
                SkillsRequired = skills,
                PersonalQualities = null,
                ImportantInformation = null,
                ApplicationInstructions = liveVacancy.ApplicationInstructions,
                ApplicationUrl = liveVacancy.ApplicationUrl,
                FutureProspects = liveVacancy.OutcomeDescription,
                ThingsToConsider = liveVacancy.ThingsToConsider,
                IsNationwide = false,
                SupplementaryQuestion1 = null,
                SupplementaryQuestion2 = null,
                VacancyUrl = $"{liveVacancyBaseUrl}/{liveVacancy.VacancyReference}",
                Location = MapFromRecruitAddress(liveVacancy.EmployerLocation),
                TrainingProviderName = liveVacancy.TrainingProvider.Name,
                TrainingProviderUkprn = liveVacancy.TrainingProvider.Ukprn.ToString(),
                TrainingProviderSite = null,
                IsEmployerDisabilityConfident = false,
                ApprenticeshipLevel = liveVacancy.ProgrammeLevel,
                TrainingType = trainingType,
                TrainingCode = trainingCode,
                TrainingTitle = trainingTitle
            };

            return apprenticeship;
        }

        private ApiTypes.WageUnit GetWageUnit(string wageType)
        {
            switch (wageType)
            {
                case FixedWageType:
                    return ApiTypes.WageUnit.Annually;
                case NationalMinimumWageWageType:
                case NationalMinimumWageForApprenticesWageType:
                    return ApiTypes.WageUnit.Weekly;
                default: //including "Unspecified"
                    return ApiTypes.WageUnit.Unspecified;
            }
        }

        private async Task<string> GetWageText(recruitEntities.Wage wage, DateTime expectedStartDate)
        {
            var wageRange = await _minimumWagesService.GetWageRangeAsync(expectedStartDate);
            switch (wage.WageType)
            {
                case NationalMinimumWageForApprenticesWageType:
                    return GetApprenticeshipMinimumWage(wageRange, wage.WeeklyHours);
                case NationalMinimumWageWageType:
                    return GetNationalMinimumWage(wageRange, wage.WeeklyHours);
                case UnspecifiedWageType:
                    return UnknownText;
                default: //including FixedWage
                    return GetFormattedCurrencyString(wage.FixedWageYearlyAmount.GetValueOrDefault());
            }
        }

        private string GetNationalMinimumWage(WageRange wageRange, decimal hoursPerWeek)
        {
            var lowerMinimumLimit = wageRange.NationalMinimumWage * hoursPerWeek;
            var upperMinimumLimit = wageRange.NationalMaximumWage * hoursPerWeek;

            var minLowerBoundSection = GetFormattedCurrencyString(lowerMinimumLimit);
            var minUpperBoundSection = GetFormattedCurrencyString(upperMinimumLimit);

            return $"{minLowerBoundSection} - {minUpperBoundSection}";
        }

        private string GetApprenticeshipMinimumWage(WageRange wageRange, decimal hoursPerWeek)
        {
            var wages = wageRange.ApprenticeMinimumWage * hoursPerWeek;

            return GetFormattedCurrencyString(wages);
        }

        private static string GetFormattedCurrencyString(decimal src)
        {
            const string currencyStringFormat = "C";
            return src.ToString(currencyStringFormat, CultureInfo.GetCultureInfo("en-GB"));
        }

        private string GetDurationAsText(recruitEntities.Wage wage)
        {
            var unit = wage.Duration == 1 ? wage.DurationUnit : $"{wage.DurationUnit}s";

            return $"{wage.Duration} {unit}";
        }

        private async Task<string> GetTrainingTitle(ApiTypes.TrainingType trainingType, string trainingCode)
        {
            var code = int.Parse(trainingCode);
            if (trainingType == ApiTypes.TrainingType.Framework)
            {

                var framework = await _trainingDetailService.GetFrameworkDetailsAsync(code);
                return framework.Title;
            }
            else
            {
                var standard = await _trainingDetailService.GetStandardDetailsAsync(code);
                return standard.Title;
            }
        }

        private string GetVacancyQualification(recruitEntities.LiveVacancy liveVacancy)
        {
            var formattedDescriptions = liveVacancy.Qualifications.Select(q => $"{q.QualificationType} {q.Subject} (Grade {q.Grade}) {q.Weighting}");
            return string.Join(",", formattedDescriptions);
        }

        private string GetTrainingCode(ApiTypes.TrainingType trainingType, string code)
        {
            var result = code;
            if (trainingType == ApiTypes.TrainingType.Framework)
            {
                result = code.Split('-')[0];
            }
            return result;
        }

        private ApiTypes.GeoCodedAddress MapFromRecruitAddress(recruitEntities.Address address)
        {
            return new ApiTypes.GeoCodedAddress()
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                AddressLine4 = address.AddressLine4,
                PostCode = address.Postcode,
                GeoPoint = new ApiTypes.GeoPoint(address.Latitude, address.Longitude)
            };
        }

    }

}