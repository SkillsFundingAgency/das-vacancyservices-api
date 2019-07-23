using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipParametersMapper : ICreateApprenticeshipParametersMapper
    {
        private readonly IWageTypeMapper _wageTypeMapper;
        private readonly IDurationMapper _durationMapper;
        private readonly IWageTextFormatter _wageTextFormatter;
        private const int StandardLocationType = 1;
        private const int NationwideLocationType = 3;

        public CreateApprenticeshipParametersMapper(IWageTypeMapper wageTypeMapper, IDurationMapper durationMapper, IWageTextFormatter wageTextFormatter)
        {
            _wageTypeMapper = wageTypeMapper;
            _durationMapper = durationMapper;
            _wageTextFormatter = wageTextFormatter;
        }

        public CreateApprenticeshipParameters MapFromRequest(CreateApprenticeshipRequest request,
            EmployerInformation employerInformation)
        {
            var parameters = new CreateApprenticeshipParameters
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Description = request.LongDescription,
                ApplicationClosingDate = request.ApplicationClosingDate.Date,
                DesiredSkills = request.DesiredSkills,
                DesiredPersonalQualities = request.DesiredPersonalQualities,
                DesiredQualifications = request.DesiredQualifications,
                FutureProspects = request.FutureProspects,
                ThingsToConsider = request.ThingsToConsider,
                TrainingToBeProvided = request.TrainingToBeProvided,
                DurationValue = request.ExpectedDuration,
                DurationTypeId = (int)_durationMapper.MapTypeToDomainType(request.DurationType),
                ExpectedStartDate = request.ExpectedStartDate.Date,
                WorkingWeek = request.WorkingWeek,
                HoursPerWeek = request.HoursPerWeek,
                WageType = (int)_wageTypeMapper.MapToLegacy(request),
                WageTypeReason = request.WageTypeReason,
                WageUnitId = (int)request.WageUnit,
                WeeklyWage = request.FixedWage,
                WageLowerBound = request.MinWage,
                WageUpperBound = request.MaxWage,
                WageText = _wageTextFormatter.GetWageText(request),
                LocationTypeId = request.LocationType == LocationType.Nationwide ? NationwideLocationType : StandardLocationType,
                NumberOfPositions = request.NumberOfPositions,
                VacancyOwnerRelationshipId = employerInformation.VacancyOwnerRelationshipId,
                EmployerDescription =
                    string.IsNullOrWhiteSpace(request.EmployerDescription)
                    ? employerInformation.EmployerDescription
                    : request.EmployerDescription,
                EmployerWebsite =
                    string.IsNullOrWhiteSpace(request.EmployerWebsiteUrl)
                    ? employerInformation.EmployerWebsite
                    : request.EmployerWebsiteUrl,
                ProviderId = employerInformation.ProviderId,
                ProviderSiteId = employerInformation.ProviderSiteId,
                ApplyOutsideNAVMS = request.ApplicationMethod == ApplicationMethod.Offline,
                SupplementaryQuestion1 = request.SupplementaryQuestion1,
                SupplementaryQuestion2 = request.SupplementaryQuestion2,
                EmployersRecruitmentWebsite = request.ExternalApplicationUrl,
                EmployersApplicationInstructions = request.ExternalApplicationInstructions,
                ContactName = request.ContactName,
                ContactEmail = request.ContactEmail,
                ContactNumber = request.ContactNumber,
                TrainingTypeId = (int)request.TrainingType,
                ApprenticeshipType = GetApprenticeshipType(request.EducationLevel),
                IsDisabilityConfident = request.IsEmployerDisabilityConfident.GetValueOrDefault(),
                AdditionalLocationInformation = request.AdditionalLocationInformation,
                HistoryUserName = request.UserEmail
            };

            MapLocationFields(request, employerInformation, parameters);

            MapTrainingCode(request, parameters);
            return parameters;
        }

        private static ApprenticeshipType GetApprenticeshipType(int educationLevel)
        {
            switch (educationLevel)
            {
                case 2:
                    return ApprenticeshipType.Intermediate;
                case 3:
                    return ApprenticeshipType.Advanced;
                case 4:
                case 5:
                    return ApprenticeshipType.Higher;
                case 6:
                case 7:
                    return ApprenticeshipType.Degree;
                default:
                    return (ApprenticeshipType)0;
            }
        }

        private static void MapLocationFields(CreateApprenticeshipRequest request, EmployerInformation employerInformation,
            CreateApprenticeshipParameters parameters)
        {
            if (request.LocationType == LocationType.OtherLocation)
            {
                parameters.AddressLine1 = request.AddressLine1;
                parameters.AddressLine2 = request.AddressLine2;
                parameters.AddressLine3 = request.AddressLine3;
                parameters.AddressLine4 = request.AddressLine4;
                parameters.AddressLine5 = request.AddressLine5;
                parameters.Town = request.Town;
                parameters.Postcode = request.Postcode;
            }
            else
            {
                parameters.AddressLine1 = employerInformation.AddressLine1;
                parameters.AddressLine2 = employerInformation.AddressLine2;
                parameters.AddressLine3 = employerInformation.AddressLine3;
                parameters.AddressLine4 = employerInformation.AddressLine4;
                parameters.AddressLine5 = employerInformation.AddressLine5;
                parameters.Town = employerInformation.Town;
                parameters.Postcode = employerInformation.Postcode;
            }
        }

        private static void MapTrainingCode(CreateApprenticeshipRequest request, CreateApprenticeshipParameters parameters)
        {
            if (request.TrainingType == TrainingType.Standard)
            {
                parameters.TrainingCode = request.TrainingCode;
            }
            else
            {
                var length = request.TrainingCode.IndexOf("-", StringComparison.Ordinal);
                parameters.TrainingCode = request.TrainingCode.Substring(0, length);
            }
        }
    }
}