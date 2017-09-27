using System;

namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// A vacancy for either an apprenticeship or a traineeship 
    /// </summary>
    public class Vacancy
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
        /// Type of the vacancy.
        /// </summary>
        public VacancyType VacancyType { get; set; }

        /// <summary>
        /// Wage unit.
        /// </summary>
        public WageUnit? WageUnit { get; set; }

        /// <summary>
        /// Weekly wage.
        /// </summary>
        public decimal? WeeklyWage { get; set; }

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
        public DateTime DatePosted { get; set; }

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
        /// URL to get Training details
        /// </summary>
        public string TrainingUri { get; set; }

        /// <summary>
        /// Employer'e name.
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
        /// Qualificatios required.
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
        /// Type of the vacancy location.
        /// </summary>
        public VacancyLocationType LocationType { get; set; }

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
        /// Gets or sets the contract owner's name.
        /// </summary>
        /// <value>
        /// The contract owner's name.
        /// </value>
        public string ContractOwner { get; set; }

        /// <summary>
        /// Gets or sets the learning provider's name
        /// </summary>
        /// <value>
        /// The learning provider's name
        /// </value>
        public string LearningProviderName { get; set; }

        /// <summary>
        /// Gets or sets the learning provider's description
        /// </summary>
        /// <value>
        /// The learning provider's description
        /// </value>
        public string LearningProviderDescription { get; set; }

        /// <summary>
        /// Gets or sets the learning provider sector pass rate
        /// </summary>
        /// <value>
        /// The learning 
        /// </value>
        public int LearningProviderSectorPassRate { get; set; }

        /// <summary>
        /// Gets or sets the delivery organisation
        /// </summary>
        /// <value>
        /// The delivery organisation
        /// </value>
        public string DeliveryOrganisation { get; set; }

        /// <summary>
        /// Gets or sets the vacancy manager
        /// </summary>
        /// <value>
        /// The vacancy manager
        /// </value>
        public string VacancyManager { get; set; }

        /// <summary>
        /// Gets or sets the vacancy owner
        /// </summary>
        /// <value>
        /// The vacancy manager
        /// </value>
        public string VacancyOwner { get; set; }

        /// <summary>
        /// Gets or sets the small employer wage incentive
        /// </summary>
        /// <value>
        /// Small employer wage incentive
        /// </value>
        public bool IsSmallEmployerWageIncentive { get; set; }

        /// <summary>
        /// When a Recruitment Agency is the Vacancy Manager, specifies whether the Recruitment Agency details are shown on the vacancy.
        /// </summary>
        /// <value>
        /// Display recruitment agency
        /// </value>
        public bool IsDisplayRecruitmentAgency { get; set; }
    }
}
