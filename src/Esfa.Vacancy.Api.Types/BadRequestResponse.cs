using System.Collections.Generic;

namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// Error response resulting from a bad request (HTTP STATUS 400)
    /// </summary>
    public class BadRequestResponse
    {
        /// <summary>
        /// Collection of errors from the bad request
        /// </summary>
        public IEnumerable<BadRequestError> RequestErrors { get; set; }
    }
}