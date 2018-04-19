using System;

namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// An apprenticeship vacancy
    /// </summary>
    public class ApprenticeshipVacancy
    {
        /// <summary>
        /// Reference number.
        /// </summary>
        public int VacancyReference { get; set; }

        /// <summary>
        /// Vacancy Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The short description of the vacancy.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// The long description of the vacancy.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Wage unit.
        /// </summary>
        public WageUnit WageUnit { get; set; }

        /// <summary>
        /// Working week.
        /// </summary>
        public string WorkingWeek { get; set; }

        /// <summary>
        /// Wage text.
        /// </summary>
        public string WageText { get; set; }

        /// <summary>
        /// Hours per week.
        /// </summary>
        public decimal? HoursPerWeek { get; set; }

        /// <summary>
        /// Expected duration.
        /// </summary>
        public string ExpectedDuration { get; set; }

        /// <summary>
        /// Expected start date.
        /// </summary>
        public DateTime? ExpectedStartDate { get; set; }

        /// <summary>
        /// Date posted.
        /// </summary>
        public DateTime PostedDate { get; set; }

        /// <summary>
        /// Application closing date.
        /// </summary>
        public DateTime? ApplicationClosingDate { get; set; }

        /// <summary>
        /// Interview from date.
        /// </summary>
        public DateTime? InterviewFromDate { get; set; }

        /// <summary>
        /// Number of positions.
        /// </summary>
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// Training Type
        /// </summary>
        public TrainingType TrainingType { get; set; }

        /// <summary>
        /// Training title.
        /// </summary>
        public string TrainingTitle { get; set; }

        /// <summary>
        /// Training identifier
        /// </summary>
        public string TrainingCode { get; set; }

        /// <summary>
        /// The name of the employer.
        /// </summary>
        public string EmployerName { get; set; }

        /// <summary>
        /// Employer's description.
        /// </summary>
        public string EmployerDescription { get; set; }

        /// <summary>
        /// Employers website.
        /// </summary>
        public string EmployerWebsite { get; set; }

        /// <summary>
        /// Training to be provided.
        /// </summary>
        public string TrainingToBeProvided { get; set; }

        /// <summary>
        /// Qualifications required.
        /// </summary>
        public string QualificationsRequired { get; set; }

        /// <summary>
        /// Skills required.
        /// </summary>
        public string SkillsRequired { get; set; }

        /// <summary>
        /// Personal qualities.
        /// </summary>
        public string PersonalQualities { get; set; }

        /// <summary>
        /// Important information.
        /// </summary>
        public string ImportantInformation { get; set; }

        /// <summary>
        /// Future prospects.
        /// </summary>
        public string FutureProspects { get; set; }

        /// <summary>
        /// Things to consider.
        /// </summary>
        public string ThingsToConsider { get; set; }

        /// <summary>
        /// Is this vacancy available nationwide.
        /// </summary>
        public bool IsNationwide { get; set; }

        /// <summary>
        /// Supplementary question1.
        /// </summary>
        public string SupplementaryQuestion1 { get; set; }

        /// <summary>
        /// Supplementary question2.
        /// </summary>
        public string SupplementaryQuestion2 { get; set; }

        /// <summary>
        /// Vacancy URL.
        /// </summary>
        public string VacancyUrl { get; set; }

        /// <summary>
        /// Contains the address of the Vacancy
        /// </summary>
        public GeoCodedAddress Location { get; set; }

        /// <summary>
        /// Name of the contact person
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Email of the contact person
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Phone number of the contact person
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// The training provider's trading name
        /// </summary>
        public string TrainingProviderName { get; set; }

        /// <summary>
        /// The training provider's UKPRN
        /// </summary>
        public string TrainingProviderUkprn { get; set; }

        /// <summary>
        /// The training provider site
        /// </summary>
        public string TrainingProviderSite { get; set; }

        /// <summary>
        /// Is employer disability confident.
        /// </summary>
        public bool IsEmployerDisabilityConfident { get; set; }

        /// <summary>
        /// Vacancy education level.
        /// </summary>
        public string EducationLevel { get; set; }
    }
}
