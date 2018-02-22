using System;

namespace Esfa.Vacancy.Api.Types
{
    public class CreateApprenticeshipParameters
    {
        /// <summary>
        /// The title of the vacancy.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description of the vacancy to be displayed in search results.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// A description of the vacancy.
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// The desired skills expected from the candidate.
        /// </summary>
        public string DesiredSkills { get; set; }

        /// <summary>
        /// The desired personal qualities expected from the candidate.
        /// </summary>
        public string DesiredPersonalQualities { get; set; }

        /// <summary>
        /// The desired qualifications for the vacancy.
        /// </summary>
        public string DesiredQualifications { get; set; }

        /// <summary>
        /// The future prospects for the vacancy.
        /// </summary>
        public string FutureProspects { get; set; }

        /// <summary>
        /// The things to consider about the vacancy.
        /// </summary>
        public string ThingsToConsider { get; set; }

        /// <summary>
        /// The training to be provided during the vacancy.
        /// </summary>
        public string TrainingToBeProvided { get; set; }

        /// <summary>
        /// The application method to be used for the vacancy.
        /// </summary>
        public ApplicationMethod ApplicationMethod { get; set; }

        /// <summary>
        /// The first supplementary question for an online vacancy.
        /// </summary>
        public string SupplementaryQuestion1 { get; set; }

        /// <summary>
        /// The second supplementary question for an online vacancy.
        /// </summary>
        public string SupplementaryQuestion2 { get; set; }

        /// <summary>
        /// The external application url for an offline vacancy.
        /// </summary>
        public string ExternalApplicationUrl { get; set; }

        /// <summary>
        /// The external application instructions for an offline vacancy.
        /// </summary>
        public string ExternalApplicationInstructions { get; set; }

        /// <summary>
        /// The expected duration of the apprenticeship.
        /// </summary>
        public int ExpectedDuration { get; set; }

        /// <summary>
        /// The type of duration - weeks, months or years.
        /// </summary>
        public DurationType DurationType { get; set; }

        /// <summary>
        /// The closing date of the application.
        /// </summary>
        public DateTime ApplicationClosingDate { get; set; }

        /// <summary>
        /// The expected start date of the apprenticeship.
        /// </summary>
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// A short explanation of the days and hours of a typical working week.
        /// </summary>
        public string WorkingWeek { get; set; }

        /// <summary>
        /// The number of hours in a typical working week.
        /// </summary>
        public double HoursPerWeek { get; set; }

        /// <summary>
        /// The wage type used for the vacancy
        /// </summary>
        public WageType WageType { get; set; }

        /// <summary>
        /// The reason for choosing the wage type of 'CompetitiveSalary', 'Unwaged' or 'ToBeSpecified'.
        /// </summary>
        public string WageTypeReason { get; set; }

        /// <summary>
        /// The frequency of payments
        /// </summary>
        public Request.WageUnit WageUnit { get; set; }

        /// <summary>
        /// The minimum wage for the vacancy
        /// </summary>
        public decimal? MinWage { get; set; }

        /// <summary>
        /// The maximum wage for the vacancy
        /// </summary>
        public decimal? MaxWage { get; set; }

        /// <summary>
        /// The fixed wage for the vacancy
        /// </summary>
        public decimal? FixedWage { get; set; }

        /// <summary>
        /// The location type used for the vacancy
        /// </summary>
        public LocationType LocationType { get; set; }

        /// <summary>
        /// The Location of the Vacancy, required in case LocationType is set to OtherLocation
        /// </summary>
        public Address Location { get; set; } = new Address();

        /// <summary>
        /// Number of positions available for the vacancy
        /// </summary>
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// Employer's unique reference number
        /// </summary>
        public int EmployerEdsUrn { get; set; }

        /// <summary>
        /// Provider site's unique reference number
        /// </summary>
        public int ProviderSiteEdsUrn { get; set; }

        /// <summary>
        /// Vacancy contact's name
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Vacancy contact's email
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Vacancy contact's phone number
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Vacancy Training Type
        /// </summary>
        public Request.TrainingType TrainingType { get; set; }

        /// <summary>
        /// Lars code for selected Training Type
        /// </summary>
        public string TrainingCode { get; set; }
    }
}