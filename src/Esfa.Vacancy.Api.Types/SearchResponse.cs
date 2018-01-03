using System.Collections.Generic;

namespace Esfa.Vacancy.Api.Types
{
    public class SearchResponse<T>
    {
        /// <summary>
        /// Total number of records that matched search criteria
        /// </summary>
        public long TotalMatched { get; set; }

        /// <summary>
        /// Number of records returned
        /// </summary>
        public long TotalReturned { get; set; }

        /// <summary>
        /// The current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public double TotalPages { get; set; }

        /// <summary>
        /// The order that the results are sorted by
        /// </summary>
        public SortBy SortBy { get; set; }

        /// <summary>
        /// Search results
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}
