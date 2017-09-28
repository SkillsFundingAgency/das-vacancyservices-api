﻿using System;

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
        public WageUnit? WageUnit { get; set; }

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

        public string EmployerName { get; set; }

        public string ProviderName { get; set; }

        public bool IsPositiveAboutDisability { get; set; }

        public bool IsEmployerAnonymous { get; set; }

        public string AnonymousEmployerName { get; set; }

        public string VacancyLocationType { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string Category { get; set; }

        public string CategoryCode { get; set; }

        public string SubCategory { get; set; }

        public string SubCategoryCode { get; set; }


    }
}
