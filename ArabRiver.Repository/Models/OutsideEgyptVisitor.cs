namespace ArabRiver.Repository.Models
{
    public class OutsideEgyptVisitor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? OrganizationName { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string IpAddress { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;
    }
}
