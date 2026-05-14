namespace ArabRiver.Service.DTOs.Lead
{
    public class CreateLeadDto
    {
        public string FirstName { get; set; }

        public string PhoneNumber { get; set; }

        public string? OrganizationName { get; set; }

        public Guid CatalogId { get; set; }
    }
}
