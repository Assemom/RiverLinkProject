namespace ArabRiver.Service.DTOs.Catalog
{
    public class CatalogResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string GoogleDriveLink { get; set; }

        public string? ThumbnailUrl { get; set; }

        public Guid? PartnerId { get; set; }

        public string? PartnerName { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
