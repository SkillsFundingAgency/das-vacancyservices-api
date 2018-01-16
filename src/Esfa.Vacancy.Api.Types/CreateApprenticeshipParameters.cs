using System;

namespace Esfa.Vacancy.Api.Types
{
    public class CreateApprenticeshipParameters
    {
        /// <summary>
        /// The title of the vacancy.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description of the vacancy to be displayed in search results.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// A description of the vacancy.
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// The closing date of the application.
        /// </summary>
        public DateTime ApplicationClosingDate { get; set; }

        /// <summary>
        /// The expected start date of the apprenticeship.
        /// </summary>
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// A short explanation of the days and hours of a typical working week.
        /// </summary>
        public string WorkingWeek { get; set; }

        /// <summary>
        /// The number of hours in a typical working week.
        /// </summary>
        public double HoursPerWeek { get; set; }
    }
}
