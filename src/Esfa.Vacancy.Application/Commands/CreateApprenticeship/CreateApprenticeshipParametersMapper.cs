using System;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Entities.CreateApprenticeship;
using Humanizer;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipParametersMapper : ICreateApprenticeshipParametersMapper
    {
        private readonly IWageTypeMapper _wageTypeMapper;
        private readonly IDurationMapper _durationMapper;
        private const int StandardLocationType = 1;
        private const int NationwideLocationType = 3;
        public CreateApprenticeshipParametersMapper(IWageTypeMapper wageTypeMapper, IDurationMapper durationMapper)
        {
            _wageTypeMapper = wageTypeMapper;
            _durationMapper = durationMapper;
        }

        public CreateApprenticeshipParameters MapFromRequest(CreateApprenticeshipRequest request,
            EmployerInformation employerInformation)
        {
            var parameters = new CreateApprenticeshipParameters
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Description = request.LongDescription,
                ApplicationClosingDate = request.ApplicationClosingDate,
                DesiredSkills = request.DesiredSkills,
                DesiredPersonalQualities = request.DesiredPersonalQualities,
                DesiredQualifications = request.DesiredQualifications,
                FutureProspects = request.FutureProspects,
                ThingsToConsider = request.ThingsToConsider,
                TrainingToBeProvided = request.TrainingToBeProvided,
                DurationValue = request.ExpectedDuration,
                DurationTypeId = (int)_durationMapper.MapTypeToDomainType(request.DurationType),
                ExpectedDuration = _durationMapper.MapToDisplayText(request.DurationType, request.ExpectedDuration),
                ExpectedStartDate = request.ExpectedStartDate,
                WorkingWeek = request.WorkingWeek,
                HoursPerWeek = request.HoursPerWeek,
                WageType = _wageTypeMapper.MapToLegacy(request.WageType),
                WageTypeReason = request.WageTypeReason,
                WageUnit = (LegacyWageUnit)request.WageUnit,
                LocationTypeId = request.LocationType == LocationType.Nationwide ? NationwideLocationType : StandardLocationType,
                NumberOfPositions = request.NumberOfPositions,
                VacancyOwnerRelationshipId = employerInformation.VacancyOwnerRelationshipId,
                EmployerDescription = employerInformation.EmployerDescription,
                EmployersWebsite = employerInformation.EmployerWebsite,
                ProviderId = employerInformation.ProviderId,
                ProviderSiteId = employerInformation.ProviderSiteId,
                OfflineVacancyTypeId = (int)request.ApplicationMethod,
                SupplementaryQuestion1 = request.SupplementaryQuestion1,
                SupplementaryQuestion2 = request.SupplementaryQuestion2,
                ContactName = request.ContactName,
                ContactEmail = request.ContactEmail,
                ContactNumber = request.ContactNumber
            };

            MapLocationFields(request, employerInformation, parameters);

            return parameters;
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
    }
}