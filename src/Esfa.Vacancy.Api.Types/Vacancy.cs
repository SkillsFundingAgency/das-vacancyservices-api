using System;

namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// A vacancy for either an apprenticeship or a traineeship 
    /// </summary>
    public class Vacancy
    {
        /// <summary>
        /// The public vacancy reference identifier for the vacancy
        /// </summary>
        public int Reference { get; set; }

        /// <summary>
        /// Vacancy Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Short Description of the vacancy
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Long description of the vacancy
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Vacancy type
        /// </summary>
        public VacancyType VacancyType { get; set; }

        /// <summary>
        /// Wage unit
        /// </summary>
        public WageUnit WageUnit { get; set; }

        /// <summary>
        /// Weekly wage
        /// </summary>
        public decimal? WeeklyWage { get; set; }

        /// <summary>
        /// Working week
        /// </summary>
        public string WorkingWeek { get; set; }

        /// <summary>
        /// Wage text
        /// </summary>
        public string WageText { get; set; }

        /// <summary>
        /// Hour per week
        /// </summary>
        public decimal? HoursPerWeek { get; set; }

        /// <summary>
        /// Duration of the vacancy
        /// </summary>
        public string ExpectedDuration { get; set; }

        /// <summary>
        /// Expected start date
        /// </summary>
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// The date when vacancy was posted
        /// </summary>
        public DateTime DatePosted { get; set; }

        /// <summary>
        /// Application closing date
        /// </summary>
        public DateTime ApplicationClosingDate { get; set; }

        /// <summary>
        /// Number of positions available
        /// </summary>
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// LARS Standard Id
        /// </summary>
        public int? StandardCode { get; set; }

        /// <summary>
        /// Framework Code
        /// </summary>
        public int? FrameworkCode { get; set; }
    }
}
