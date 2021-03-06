﻿using Esfa.Vacancy.Domain.Entities;
using ApplicationTypes = Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using CreateApprenticeshipParameters = Esfa.Vacancy.Api.Types.Request.CreateApprenticeshipParameters;

namespace Esfa.Vacancy.Manage.Api.Mappings
{
    public class CreateApprenticeshipRequestMapper : ICreateApprenticeshipRequestMapper
    {
        public ApplicationTypes.CreateApprenticeshipRequest MapFromApiParameters(
            CreateApprenticeshipParameters parameters,
            int providerUkprn,
            string userEmail)
        {
            return new ApplicationTypes.CreateApprenticeshipRequest
            {
                Title = parameters.Title,
                ShortDescription = parameters.ShortDescription,
                LongDescription = parameters.LongDescription,
                DesiredSkills = parameters.DesiredSkills,
                DesiredPersonalQualities = parameters.DesiredPersonalQualities,
                DesiredQualifications = parameters.DesiredQualifications,
                FutureProspects = parameters.FutureProspects,
                ThingsToConsider = parameters.ThingsToConsider,
                TrainingToBeProvided = parameters.TrainingToBeProvided,
                ApplicationMethod = (ApplicationTypes.ApplicationMethod)parameters.ApplicationMethod,
                SupplementaryQuestion1 = parameters.SupplementaryQuestion1,
                SupplementaryQuestion2 = parameters.SupplementaryQuestion2,
                ExternalApplicationUrl = parameters.ExternalApplicationUrl,
                ExternalApplicationInstructions = parameters.ExternalApplicationInstructions,
                ExpectedDuration = parameters.ExpectedDuration,
                DurationType = (ApplicationTypes.DurationType)parameters.DurationType,
                ApplicationClosingDate = parameters.ApplicationClosingDate,
                ExpectedStartDate = parameters.ExpectedStartDate,
                WorkingWeek = parameters.WorkingWeek,
                HoursPerWeek = parameters.HoursPerWeek,
                WageType = (ApplicationTypes.WageType)parameters.WageType,
                WageTypeReason = parameters.WageTypeReason,
                WageUnit = (ApplicationTypes.WageUnit)parameters.WageUnit,
                FixedWage = parameters.FixedWage,
                MinWage = parameters.MinWage,
                MaxWage = parameters.MaxWage,
                LocationType = (ApplicationTypes.LocationType)parameters.LocationType,
                AddressLine1 = parameters.Location.AddressLine1,
                AddressLine2 = parameters.Location.AddressLine2,
                AddressLine3 = parameters.Location.AddressLine3,
                AddressLine4 = parameters.Location.AddressLine4,
                AddressLine5 = parameters.Location.AddressLine5,
                Town = parameters.Location.Town,
                Postcode = parameters.Location.Postcode,
                NumberOfPositions = parameters.NumberOfPositions,
                ProviderUkprn = providerUkprn,
                EmployerEdsUrn = parameters.EmployerEdsUrn,
                ProviderSiteEdsUrn = parameters.ProviderSiteEdsUrn,
                ContactName = parameters.ContactName,
                ContactEmail = parameters.ContactEmail,
                ContactNumber = parameters.ContactNumber,
                TrainingType = (TrainingType)parameters.TrainingType,
                TrainingCode = parameters.TrainingCode,
                IsEmployerDisabilityConfident = parameters.IsEmployerDisabilityConfident,
                AdditionalLocationInformation = parameters.Location.AdditionalInformation,
                UserEmail = userEmail,
                EmployerDescription = parameters.EmployerDescription,
                EmployerWebsiteUrl = parameters.EmployerWebsiteUrl
            };
        }
    }
}
