using System;
using Esfa.Vacancy.Domain.Entities.CreateApprenticeship;

namespace Esfa.Vacancy.Domain.Entities
{
    public class CreateApprenticeshipParameters
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime ApplicationClosingDate { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public string WorkingWeek { get; set; }
        public double HoursPerWeek { get; set; }
        public LegacyWageType WageType { get; set; }
        public string WageTypeReason { get; set; }
        public LegacyWageUnit WageUnit { get; set; }
        public decimal? WageLowerBound { get; set; }
        public decimal? WageUpperBound { get; set; }
        public int LocationTypeId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public int NumberOfPositions { get; set; }
        public int VacancyOwnerRelationshipId { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployersWebsite { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public string DesiredSkills { get; set; }
        public string DesiredPersonalQualities { get; set; }
        public string DesiredQualifications { get; set; }
        public string FutureProspects { get; set; }
        public string ThingsToConsider { get; set; }
        public string TrainingToBeProvided { get; set; }
        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }
        public string ExpectedDuration { get; set; }
        public int DurationTypeId { get; set; }
        public int DurationValue { get; set; }
        public bool ApplyOutsideNAVMS { get; set; }
        public string SupplementaryQuestion1 { get; set; }
        public string SupplementaryQuestion2 { get; set; }
        public string EmployersRecruitmentWebsite { get; set; }
        public string EmployersApplicationInstructions { get; set; }
    }
}