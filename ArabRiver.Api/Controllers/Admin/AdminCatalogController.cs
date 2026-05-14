using ArabRiver.Api.Models.Catalog;
using ArabRiver.Service.DTOs;
using ArabRiver.Service.DTOs.Catalog;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArabRiver.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/catalogs")]
    [Authorize]
    public class AdminCatalogController : ControllerBase
    {
        private readonly ICatalogService
            _catalogService;
        private readonly IWebHostEnvironment _environment;

        public AdminCatalogController(
            ICatalogService catalogService,
            IWebHostEnvironment environment)
        {
            _catalogService = catalogService;
            _environment = environment;
        }

        // =========================================
        // Get All Catalogs (Admin)
        // =========================================

        [HttpGet]
        public async Task<IActionResult>
            GetAllCatalogs(
                [FromQuery]
            PaginationParameters parameters,
                [FromQuery] Guid? partnerId)
        {
            var result =
                await _catalogService
                    .GetAllCatalogsAsync(
                        parameters,
                        partnerId);

            return Ok(result);
        }

        // =========================================
        // Create Catalog
        // =========================================

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>
            CreateCatalog(
                [FromForm] CreateCatalogRequest request)
        {
            string? thumbnailUrl = null;

            if (request.ThumbnailFile is not null
                && request.ThumbnailFile.Length > 0)
            {
                var webRoot = _environment.WebRootPath
                    ?? Path.Combine(
                        _environment.ContentRootPath,
                        "wwwroot");

                var imagesFolder = Path.Combine(
                    webRoot,
                    "images",
                    "catalogs");

                Directory.CreateDirectory(imagesFolder);

                var fileExtension =
                    Path.GetExtension(
                        request.ThumbnailFile.FileName);

                var fileName =
                    $"{Guid.NewGuid()}{fileExtension}";

                var filePath =
                    Path.Combine(imagesFolder, fileName);

                await using (var stream =
                             new FileStream(
                                 filePath,
                                 FileMode.Create))
                {
                    await request.ThumbnailFile
                        .CopyToAsync(stream);
                }

                thumbnailUrl =
                    $"/images/catalogs/{fileName}";
            }

            var dto = new CreateCatalogDto
            {
                Name = request.Name,
                Description = request.Description,
                GoogleDriveLink = request.GoogleDriveLink,
                PartnerId = request.PartnerId,
                DisplayOrder = request.DisplayOrder,
                ThumbnailUrl = thumbnailUrl
            };

            var result =
                await _catalogService
                    .CreateCatalogAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // =========================================
        // Update Catalog
        // =========================================

        [HttpPut("{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult>
            UpdateCatalog(
                Guid id,
                [FromForm] UpdateCatalogRequest request)
        {
            string? thumbnailUrl = null;

            if (request.ThumbnailFile is not null
                && request.ThumbnailFile.Length > 0)
            {
                var webRoot = _environment.WebRootPath
                    ?? Path.Combine(
                        _environment.ContentRootPath,
                        "wwwroot");

                var imagesFolder = Path.Combine(
                    webRoot,
                    "images",
                    "catalogs");

                Directory.CreateDirectory(imagesFolder);

                var fileExtension =
                    Path.GetExtension(
                        request.ThumbnailFile.FileName);

                var fileName =
                    $"{Guid.NewGuid()}{fileExtension}";

                var filePath =
                    Path.Combine(imagesFolder, fileName);

                await using (var stream =
                             new FileStream(
                                 filePath,
                                 FileMode.Create))
                {
                    await request.ThumbnailFile
                        .CopyToAsync(stream);
                }

                thumbnailUrl =
                    $"/images/catalogs/{fileName}";
            }

            var dto = new UpdateCatalogDto
            {
                Name = request.Name,
                Description = request.Description,
                GoogleDriveLink = request.GoogleDriveLink,
                PartnerId = request.PartnerId,
                DisplayOrder = request.DisplayOrder,
                ThumbnailUrl = thumbnailUrl
            };

            var result =
                await _catalogService
                    .UpdateCatalogAsync(
                        id,
                        dto);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // =========================================
        // Activate / Deactivate Catalog
        // =========================================

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult>
            UpdateStatus(
                Guid id,
                CatalogStatusDto dto)
        {
            var result =
                await _catalogService
                    .UpdateCatalogStatusAsync(
                        id,
                        dto.IsActive);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
