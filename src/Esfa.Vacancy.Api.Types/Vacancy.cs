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
        public int VacancyReference { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the vacancy.
        /// </summary>
        public VacancyType VacancyType { get; set; }

        /// <summary>
        /// Gets or sets the wage unit.
        /// </summary>
        public WageUnit? WageUnit { get; set; }

        /// <summary>
        /// Gets or sets the weekly wage.
        /// </summary>
        public decimal? WeeklyWage { get; set; }

        /// <summary>
        /// Gets or sets the working week.
        /// </summary>
        public string WorkingWeek { get; set; }

        /// <summary>
        /// Gets or sets the wage text.
        /// </summary>
        public string WageText { get; set; }

        /// <summary>
        /// Gets or sets the hours per week.
        /// </summary>
        public decimal? HoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the expected duration.
        /// </summary>
        public string ExpectedDuration { get; set; }

        /// <summary>
        /// Gets or sets the expected start date.
        /// </summary>
        public DateTime? ExpectedStartDate { get; set; }

        /// <summary>
        /// Gets or sets the date posted.
        /// </summary>
        public DateTime DatePosted { get; set; }

        /// <summary>
        /// Gets or sets the application closing date.
        /// </summary>
        public DateTime? ApplicationClosingDate { get; set; }

        /// <summary>
        /// Gets or sets the interview from date.
        /// </summary>
        public DateTime? InterviewFromDate { get; set; }

        /// <summary>
        /// Gets or sets the number of positions.
        /// </summary>
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// Training Type
        /// </summary>
        public TrainingType? TrainingType { get; set; }

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
        /// Gets or sets the employers website.
        /// </summary>
        public string EmployerWebsite { get; set; }

        /// <summary>
        /// Gets or sets the training to be provided.
        /// </summary>
        public string TrainingToBeProvided { get; set; }

        /// <summary>
        /// Gets or sets the qulificatios required.
        /// </summary>
        public string QualificationsRequired { get; set; }

        /// <summary>
        /// Gets or sets the skills required.
        /// </summary>
        public string SkillsRequired { get; set; }

        /// <summary>
        /// Gets or sets the personal qualities.
        /// </summary>
        public string PersonalQualities { get; set; }

        /// <summary>
        /// Gets or sets the important information.
        /// </summary>
        public string ImportantInformation { get; set; }

        /// <summary>
        /// Gets or sets the future prospects.
        /// </summary>
        public string FutureProspects { get; set; }

        /// <summary>
        /// Gets or sets the things to consider.
        /// </summary>
        public string ThingsToConsider { get; set; }

        /// <summary>
        /// Gets or sets the type of the vacancy location.
        /// </summary>
        public VacancyLocationType LocationType { get; set; }

        /// <summary>
        /// Gets or sets the supplementary question1.
        /// </summary>
        public string SupplementaryQuestion1 { get; set; }

        /// <summary>
        /// Gets or sets the supplementary question2.
        /// </summary>
        public string SupplementaryQuestion2 { get; set; }

        /// <summary>
        /// Gets or sets the vacancy URL.
        /// </summary>
        public string VacancyUrl { get; set; }

        /// <summary>
        /// Contains the address of the Vacancy
        /// </summary>
        public Address Location { get; set; }
    }
}
