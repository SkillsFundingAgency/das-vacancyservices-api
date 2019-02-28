namespace Esfa.Vacancy.Api.Types
{
    public class SearchApprenticeshipParameters
    {
        /// <summary>
        /// Comma delimited standard codes
        /// </summary>
        public string StandardLarsCodes { get; set; }

        /// <summary>
        /// Comma delimited framework codes
        /// </summary>
        public string FrameworkLarsCodes { get; set; }

        /// <summary>
        /// The training provider's unique provider reference number.
        /// The unique provider reference number is optional and if specified it must be 8 digits in length.
        /// </summary>
        public int? Ukprn { get; set; }

        /// <summary>
        /// Number of records you want to retrieve in a page. 
        /// Page size must be between 1 and 250 (inclusive).
        /// Page size is optional and defaults to 100 if not specified.
        /// </summary>
        public int PageSize { get; set; } = 100;

        /// <summary>
        /// The page number to be retrieved. 
        /// Page number is optional and if not specified, by default the first page is served.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Vacancies posted in last number of days from today.
        /// </summary>
        public int? PostedInLastNumberOfDays { get; set; }

        /// <summary>
        /// Nationwide vacancies only.
        /// Nationwide only is optional and defaults to false.
        /// </summary>
        public bool NationwideOnly { get; set; }

        /// <summary>
        /// The latitude to search near
        /// Latitude is optional.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// The longitude to search near
        /// Longitude is optional.
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// The distance (in miles) to search from the geo-point
        /// DistanceInMiles is optional.
        /// </summary>
        public int? DistanceInMiles { get; set; }

        /// <summary>
        /// The order that the results are to be sorted by.
        /// Defaults to distance for geo-location search and age for any other search.
        /// </summary>
        public string SortBy { get; set; }
    }
}
