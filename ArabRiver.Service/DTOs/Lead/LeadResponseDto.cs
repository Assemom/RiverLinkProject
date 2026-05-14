namespace ArabRiver.Service.DTOs.Lead
{
    public class LeadResponseDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string PhoneNumber { get; set; }

        public string? OrganizationName { get; set; }

        public string Country { get; set; }

        public bool IsEgypt { get; set; }

        public string CatalogNameSnapshot { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
