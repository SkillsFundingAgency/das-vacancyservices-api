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
        /// Search results
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}
