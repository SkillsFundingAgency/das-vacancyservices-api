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
        /// The short description of the vacancy.
        /// </summary>
        public string ShortDescription { get; set; }

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
        public string TrainingProviderName { get; set; }

        /// <summary>
        /// Is this vacancy available nationwide.
        /// </summary>
        public bool IsNationwide { get; set; }

        /// <summary>
        /// The location.
        /// </summary>
        public GeoPoint Location { get; set; }

        /// <summary>
        /// The apprenticeship level.
        /// </summary>
        public string ApprenticeshipLevel { get; set; }

        /// <summary>
        /// The FAA vacancy URL.
        /// </summary>
        public string VacancyUrl { get; set; }

        /// <summary>
        /// Is employer disability confident.
        /// </summary>
        public bool IsEmployerDisabilityConfident { get; set; }

        /// <summary>
        /// The distance in miles from the geo-location based search
        /// </summary>
        public double? DistanceInMiles { get; set; }

        public ApprenticeshipSummary()
        {
            Location = new GeoPoint();
        }
    }
}
