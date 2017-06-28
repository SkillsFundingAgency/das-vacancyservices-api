namespace Esfa.Vacancy.Api.Types
{
    /// <summary>
    /// A vacancy for either an apprenticeship or a traineeship 
    /// </summary>
    public class Vacancy
    {
        /// <summary>
        /// The public reference for the Vacancy
        /// </summary>
        public int Reference { get; set; }
        /// <summary>
        /// The Uri find the vacancy on this service
        /// </summary>
        public string Uri { get; set; }
    }
}