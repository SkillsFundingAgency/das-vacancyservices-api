using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using ApplicationTypes = Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.Manage.Api.Mappings
{
    public class CreateApprenticeshipRequestMapper : ICreateApprenticeshipRequestMapper
    {
        public CreateApprenticeshipRequest MapFromApiParameters(
            CreateApprenticeshipParameters parameters,
            int providerUkprn)
        {
            return new CreateApprenticeshipRequest
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
                ExpectedDuration = parameters.ExpectedDuration,
                DurationType = (ApplicationTypes.DurationType)parameters.DurationType,
                ApplicationClosingDate = parameters.ApplicationClosingDate,
                ExpectedStartDate = parameters.ExpectedStartDate,
                WorkingWeek = parameters.WorkingWeek,
                HoursPerWeek = parameters.HoursPerWeek,
                WageType = (ApplicationTypes.WageType)parameters.WageType,
                WageTypeReason = parameters.WageTypeReason,
                WageUnit = (ApplicationTypes.WageUnit)parameters.WageUnit,
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
                ContactNumber = parameters.ContactNumber
            };
        }
    }
}