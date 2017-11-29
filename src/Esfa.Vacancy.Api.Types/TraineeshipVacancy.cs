using System;

namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// A traineeship vacancy
    /// </summary>
    public class TraineeshipVacancy
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
        /// Short description.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Working week.
        /// </summary>
        public string WorkingWeek { get; set; }

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
        /// Sector category of traineeship.
        /// </summary>
        public string TraineeshipSector { get; set; }

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
        public Address Location { get; set; }

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
    }
}
