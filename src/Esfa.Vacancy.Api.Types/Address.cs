namespace Esfa.Vacancy.Api.Types
{
    public class Address
    {
        /// <summary>
        /// The address line1.
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Gets or sets the address line3.
        /// </summary>
        public string AddressLine3 { get; set; }
        /// <summary>
        /// Gets or sets the address line4.
        /// </summary>
        public string AddressLine4 { get; set; }
        /// <summary>
        /// Gets or sets the address line5.
        /// </summary>
        public string AddressLine5 { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// Gets or sets the town.
        /// </summary>
        public string Town { get; set; }
    }
}
