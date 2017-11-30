namespace Esfa.Vacancy.Domain.Entities
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }
    }
}
