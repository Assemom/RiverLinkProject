namespace ArabRiver.Repository.Models
{
    public class Catalog
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string GoogleDriveLink { get; set; }

        public string? ThumbnailUrl { get; set; }

        public Guid? PartnerId { get; set; }

        public Partner? Partner { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
