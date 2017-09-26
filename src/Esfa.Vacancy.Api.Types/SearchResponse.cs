using System.Collections.Generic;

namespace Esfa.Vacancy.Api.Types
{
    public class SearchResponse<T>
    {
        /// <summary>
        /// Search results.
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}
