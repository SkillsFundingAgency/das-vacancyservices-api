﻿namespace Esfa.Vacancy.Register.Domain.Entities
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string PostCode { get; set; }
        public string Town { get; set; }
    }
}
