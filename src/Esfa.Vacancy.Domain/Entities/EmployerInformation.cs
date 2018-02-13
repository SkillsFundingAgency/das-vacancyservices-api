namespace Esfa.Vacancy.Domain.Entities
{
    public class EmployerInformation
    {
        public int VacancyOwnerRelationshipId { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployerWebsite { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string Town { get; set; }

        public string Postcode { get; set; }

        public int ProviderId { get; set; }

        public int ProviderSiteId { get; set; }
    }
}
