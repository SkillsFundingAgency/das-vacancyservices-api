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
    }
}
