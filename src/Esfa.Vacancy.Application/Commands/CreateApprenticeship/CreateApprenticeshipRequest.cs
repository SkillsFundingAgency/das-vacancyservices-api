using System;
using Esfa.Vacancy.Domain.Entities;
using MediatR;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipRequest : IRequest<CreateApprenticeshipResponse>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string DesiredSkills { get; set; }
        public string DesiredPersonalQualities { get; set; }
        public string DesiredQualifications { get; set; }
        public string FutureProspects { get; set; }
        public string ThingsToConsider { get; set; }
        public DateTime ApplicationClosingDate { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public string WorkingWeek { get; set; }
        public double HoursPerWeek { get; set; }
        public WageType WageType { get; set; }
        public string WageTypeReason { get; set; }
        public WageUnit WageUnit { get; set; }
        public decimal? MinWage { get; set; }
        public decimal? MaxWage { get; set; }
        public decimal? FixedWage { get; set; }
        public LocationType LocationType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public int NumberOfPositions { get; set; }
        public int ProviderUkprn { get; set; }
        public int EmployerEdsUrn { get; set; }
        public int ProviderSiteEdsUrn { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public string TrainingToBeProvided { get; set; }
        public ApplicationMethod ApplicationMethod { get; set; }
        public string SupplementaryQuestion1 { get; set; }
        public string SupplementaryQuestion2 { get; set; }
        public string ExternalApplicationUrl { get; set; }
        public string ExternalApplicationInstructions { get; set; }
        public int ExpectedDuration { get; set; }
        public DurationType DurationType { get; set; }
        public TrainingType TrainingType { get; set; }
        public string TrainingCode { get; set; }
    }
}
