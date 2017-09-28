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

        public DateTime StartDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public DateTime PostedDate { get; set; }

        public string EmployerName { get; set; }

        public string ProviderName { get; set; }

        public string Description { get; set; }

        public int NumberOfPositions { get; set; }

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

        public string WorkingWeek { get; set; }

        public int WageType { get; set; }

        public decimal? WageAmount { get; set; }

        public decimal? WageAmountLowerBound { get; set; }

        public decimal? WageAmountUpperBound { get; set; }

        public string WageText { get; set; }

        public int WageUnit { get; set; }

        public decimal? HoursPerWeek { get; set; }
    }
}
