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
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// Gets or sets the date posted.
        /// </summary>
        /// <value>
        /// The date posted.
        /// </value>
        public DateTime DatePosted { get; set; }

        /// <summary>
        /// Gets or sets the application closing date.
        /// </summary>
        /// <value>
        /// The application closing date.
        /// </value>
        public DateTime ApplicationClosingDate { get; set; }

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
        /// Gets or sets the framework code.
        /// </summary>
        /// <value>
        /// The framework code.
        /// </value>
        public int? FrameworkCode { get; set; }

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
        public string EmployersWebsite { get; set; }

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
        public string QulificatiosRequired { get; set; }

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
        /// Gets or sets the reality check.
        /// </summary>
        /// <value>
        /// The reality check.
        /// </value>
        public string RealityCheck { get; set; }
    }
}
