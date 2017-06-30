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
        /// The Url to find the vacancy on this service
        /// </summary>
        public string Url { get; set; }

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
        /// Weekly wage
        /// </summary>
        public decimal WeeklyWage { get; set; }

        /// <summary>
        /// Working week
        /// </summary>
        public string WorkingWeek { get; set; }

        /// <summary>
        /// Hour per week
        /// </summary>
        public decimal HoursPerWeek { get; set; }

        /// <summary>
        /// Duration of the vacancy
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Expected start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date when vacancy was posted
        /// </summary>
        public DateTime DatePosted { get; set; }

        /// <summary>
        /// Number of positions available
        /// </summary>
        public int NumberofPositions { get; set; }


    }
}
