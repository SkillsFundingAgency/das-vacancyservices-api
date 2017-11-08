using System;

namespace Esfa.Vacancy.Api.Types
{
    public class ApprenticeshipSummary
    {
        /// <summary>
        /// Vacancy reference number
        /// </summary>
        public int VacancyReference { get; set; }

        /// <summary>
        /// Vacancy title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description.
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
        /// The type of the wage.
        /// </summary>
        public int WageType { get; set; }

        /// <summary>
        /// The wage amount.
        /// </summary>
        public decimal? WageAmount { get; set; }

        /// <summary>
        /// Expected start date.
        /// </summary>
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// Date Posted.
        /// </summary>
        public DateTime PostedDate { get; set; }

        /// <summary>
        /// Application closing date.
        /// </summary>
        public DateTime ApplicationClosingDate { get; set; }

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
        /// The name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        public string ProviderName { get; set; }

        /// <summary>
        /// The type of the vacancy location.
        /// </summary>
        public string VacancyLocationType { get; set; }

        /// <summary>
        /// The location.
        /// </summary>
        public GeoPoint Location { get; set; }

        /// <summary>
        /// The apprenticeship level.
        /// </summary>
        public string ApprenticeshipLevel { get; set; }

        public ApprenticeshipSummary()
        {
            Location = new GeoPoint();
        }
    }
}
