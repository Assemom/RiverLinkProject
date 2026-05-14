namespace ArabRiver.Repository.Models
{
    public class Lead
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string PhoneNumber { get; set; }

        public string? OrganizationName { get; set; }

        public Guid CatalogId { get; set; }

        public string CatalogNameSnapshot { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string IpAddress { get; set; }

        public bool IsEgypt { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        // Navigation Property
        public Catalog Catalog { get; set; }
    }
}
