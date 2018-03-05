namespace Esfa.Vacancy.Api.Types.Request
{
    public class Location
    {
        /// <summary>
        /// Address line1.
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Address line2.
        /// </summary>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Address line3.
        /// </summary>
        public string AddressLine3 { get; set; }
        /// <summary>
        /// Address line4.
        /// </summary>
        public string AddressLine4 { get; set; }
        /// <summary>
        /// Address line5.
        /// </summary>
        public string AddressLine5 { get; set; }
        /// <summary>
        /// Post code.
        /// </summary>
        public string Postcode { get; set; }
        /// <summary>
        /// Town.
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// Additional information for location other than Employer's address
        /// </summary>
        public string AdditionalInformation { get; set; }
    }
}