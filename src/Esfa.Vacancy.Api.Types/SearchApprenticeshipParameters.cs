namespace Esfa.Vacancy.Api.Types
{
    public class SearchApprenticeshipParameters
    {
        /// <summary>
        /// Comma delimited standard codes
        /// </summary>
        public string StandardCodes { get; set; }

        /// <summary>
        /// Comma delimited framework codes
        /// </summary>
        public string FrameworkCodes { get; set; }

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
    }
}
