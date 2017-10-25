namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// Error details resulting from a bad request (HTTP STATUS 400)
    /// </summary>
    public class BadRequestError
    {
        /// <summary>
        /// Vacancy API error code
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// Descriptive message about the error
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}