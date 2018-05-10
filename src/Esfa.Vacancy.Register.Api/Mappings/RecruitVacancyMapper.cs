using System;
using System.Linq;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using ApiTypes = Esfa.Vacancy.Api.Types;
using recruitEntities = SFA.DAS.Recruit.Vacancies.Client.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class RecruitVacancyMapper : IRecruitVacancyMapper
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ITrainingDetailService _trainingDetailService;

        public RecruitVacancyMapper(IProvideSettings provideSettings, ITrainingDetailService trainingDetailService)
        {
            _provideSettings = provideSettings;
            _trainingDetailService = trainingDetailService;
        }

        public ApiTypes.ApprenticeshipVacancy MapFromRecruitVacancy(recruitEntities.LiveVacancy liveVacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeys.LiveApprenticeshipVacancyBaseUrlKey);

            var description = GetVacancyDescription(liveVacancy);

            var trainingType =
                (ApiTypes.TrainingType)Enum.Parse(typeof(ApiTypes.TrainingType), liveVacancy.ProgrammeType);

            var trainingCode = GetTrainingCode(trainingType, liveVacancy.ProgrammeId);

            var trainingTitle = GetTrainingTitle(trainingType, trainingCode);

            var qualifications = GetVacancyQualification(liveVacancy);

            var skills = string.Join(",", liveVacancy.Skills);

            var duration = GetDurationAsText(liveVacancy.Wage);

            var apprenticeship = new ApiTypes.ApprenticeshipVacancy
            {
                VacancyReference = liveVacancy.VacancyReference,
                Title = liveVacancy.Title,
                ShortDescription = liveVacancy.ShortDescription,
                Description = description,
                WageUnit = ApiTypes.WageUnit.Annually,
                WorkingWeek = liveVacancy.Wage.WorkingWeekDescription,
                //WageText = liveVacancy.Wage.FixedWageYearlyAmount, //TODO write function to work out wage text depending on wage type
                WageAdditionalInformation = liveVacancy.Wage.WageAdditionalInformation,
                HoursPerWeek = liveVacancy.Wage.WeeklyHours,
                ExpectedDuration = duration,
                ExpectedStartDate = liveVacancy.StartDate,
                PostedDate = liveVacancy.LiveDate,
                ApplicationClosingDate = liveVacancy.ClosingDate,
                NumberOfPositions = liveVacancy.NumberOfPositions,
                EmployerName = liveVacancy.EmployerContactName,
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

        private string GetDurationAsText(recruitEntities.Wage wage)
        {
            var unit = wage.Duration == 1 ? wage.DurationUnit : $"{wage.DurationUnit}s";

            return $"{wage.Duration} {unit}";
        }

        private string GetTrainingTitle(ApiTypes.TrainingType trainingType, string trainingCode)
        {
            var code = int.Parse(trainingCode);
            if (trainingType == ApiTypes.TrainingType.Framework)
            {

                var framework = _trainingDetailService.GetFrameworkDetailsAsync(code).Result;
                return framework.Title;
            }
            else
            {
                var standard = _trainingDetailService.GetStandardDetailsAsync(code).Result;
                return standard.Title;
            }
        }

        private string GetVacancyDescription(recruitEntities.LiveVacancy liveVacancy)
        {
            return string.Concat(liveVacancy.Description, Environment.NewLine, liveVacancy.TrainingDescription);
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