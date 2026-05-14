using Microsoft.AspNetCore.Http;

namespace ArabRiver.Api.Models.Catalog
{
    public class CreateCatalogRequest
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public string GoogleDriveLink { get; set; }

        public Guid? PartnerId { get; set; }

        public int DisplayOrder { get; set; }

        public IFormFile? ThumbnailFile { get; set; }
    }
}
