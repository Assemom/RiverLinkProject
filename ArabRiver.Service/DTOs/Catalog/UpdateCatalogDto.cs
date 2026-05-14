namespace ArabRiver.Service.DTOs.Catalog
{
    public class UpdateCatalogDto
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public string GoogleDriveLink { get; set; }

        public string? ThumbnailUrl { get; set; }

        public Guid? PartnerId { get; set; }

        public int DisplayOrder { get; set; }
    }
}
