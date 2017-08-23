using System;

namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// A vacancy for either an apprenticeship or a traineeship 
    /// </summary>
    public class Vacancy
    {
        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>
        /// The reference.
        /// </value>
        public int Reference { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        /// <value>
        /// The short description.
        /// </value>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the vacancy.
        /// </summary>
        /// <value>
        /// The type of the vacancy.
        /// </value>
        public VacancyType VacancyType { get; set; }

        /// <summary>
        /// Gets or sets the wage unit.
        /// </summary>
        /// <value>
        /// The wage unit.
        /// </value>
        public WageUnit WageUnit { get; set; }

        /// <summary>
        /// Gets or sets the weekly wage.
        /// </summary>
        /// <value>
        /// The weekly wage.
        /// </value>
        public decimal? WeeklyWage { get; set; }

        /// <summary>
        /// Gets or sets the working week.
        /// </summary>
        /// <value>
        /// The working week.
        /// </value>
        public string WorkingWeek { get; set; }

        /// <summary>
        /// Gets or sets the wage text.
        /// </summary>
        /// <value>
        /// The wage text.
        /// </value>
        public string WageText { get; set; }

        /// <summary>
        /// Gets or sets the hours per week.
        /// </summary>
        /// <value>
        /// The hours per week.
        /// </value>
        public decimal? HoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the expected duration.
        /// </summary>
        /// <value>
        /// The expected duration.
        /// </value>
        public string ExpectedDuration { get; set; }

        /// <summary>
        /// Gets or sets the expected start date.
        /// </summary>
        /// <value>
        /// The expected start date.
        /// </value>
        public DateTime? ExpectedStartDate { get; set; }

        /// <summary>
        /// Gets or sets the date posted.
        /// </summary>
        /// <value>
        /// The date posted.
        /// </value>
        public DateTime? DatePosted { get; set; }

        /// <summary>
        /// Gets or sets the application closing date.
        /// </summary>
        /// <value>
        /// The application closing date.
        /// </value>
        public DateTime? ApplicationClosingDate { get; set; }

        /// <summary>
        /// Gets or sets the interview from date.
        /// </summary>
        /// <value>
        /// The interview from date.
        /// </value>
        public DateTime? InterviewFromDate { get; set; }

        /// <summary>
        /// Gets or sets the number of positions.
        /// </summary>
        /// <value>
        /// The number of positions.
        /// </value>
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// Gets or sets the standard code.
        /// </summary>
        /// <value>
        /// The standard code.
        /// </value>
        public int? StandardCode { get; set; }

        /// <summary>
        /// Gets or sets the standard title.
        /// </summary>
        /// <value>
        /// The standard title.
        /// </value>
        public string StandardTitle { get; set; }

        /// <summary>
        /// Gets or sets the framework code.
        /// </summary>
        /// <value>
        /// The framework code.
        /// </value>
        public int? FrameworkCode { get; set; }

        /// <summary>
        /// Gets or sets the framework title.
        /// </summary>
        /// <value>
        /// The framework title.
        /// </value>
        public string FrameworkTitle { get; set; }

        /// <summary>
        /// Gets or sets the name of the employer.
        /// </summary>
        /// <value>
        /// The name of the employer.
        /// </value>
        public string EmployerName { get; set; }

        /// <summary>
        /// Gets or sets the employer description.
        /// </summary>
        /// <value>
        /// The employer description.
        /// </value>
        public string EmployerDescription { get; set; }

        /// <summary>
        /// Gets or sets the employers website.
        /// </summary>
        /// <value>
        /// The employers website.
        /// </value>
        public string EmployerWebsite { get; set; }

        /// <summary>
        /// Gets or sets the training to be provided.
        /// </summary>
        /// <value>
        /// The training to be provided.
        /// </value>
        public string TrainingToBeProvided { get; set; }

        /// <summary>
        /// Gets or sets the qulificatios required.
        /// </summary>
        /// <value>
        /// The qulificatios required.
        /// </value>
        public string QualificationsRequired { get; set; }

        /// <summary>
        /// Gets or sets the skills required.
        /// </summary>
        /// <value>
        /// The skills required.
        /// </value>
        public string SkillsRequired { get; set; }

        /// <summary>
        /// Gets or sets the personal qualities.
        /// </summary>
        /// <value>
        /// The personal qualities.
        /// </value>
        public string PersonalQualities { get; set; }

        /// <summary>
        /// Gets or sets the important information.
        /// </summary>
        /// <value>
        /// The important information.
        /// </value>
        public string ImportantInformation { get; set; }

        /// <summary>
        /// Gets or sets the future prospects.
        /// </summary>
        /// <value>
        /// The future prospects.
        /// </value>
        public string FutureProspects { get; set; }

        /// <summary>
        /// Gets or sets the things to consider.
        /// </summary>
        /// <value>
        /// The things to consider.
        /// </value>
        public string ThingsToConsider { get; set; }

        /// <summary>
        /// Gets or sets the type of the vacancy location.
        /// </summary>
        /// <value>
        /// The type of the vacancy location.
        /// </value>
        public VacancyLocationType VacancyLocationType { get; set; }

        /// <summary>
        /// Gets or sets the supplementary question1.
        /// </summary>
        /// <value>
        /// The supplementary question1.
        /// </value>
        public string SupplementaryQuestion1 { get; set; }

        /// <summary>
        /// Gets or sets the supplementary question2.
        /// </summary>
        /// <value>
        /// The supplementary question2.
        /// </value>
        public string SupplementaryQuestion2 { get; set; }

        /// <summary>
        /// Gets or sets the employer address.
        /// </summary>
        /// <value>
        /// The employer address.
        /// </value>
        public Address EmployerAddress { get; set; }

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
