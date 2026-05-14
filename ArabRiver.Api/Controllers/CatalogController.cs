using ArabRiver.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArabRiver.Api.Controllers
{
    [ApiController]
    [Route("api/catalogs")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService
            _catalogService;

        public CatalogController(
            ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // =========================================
        // Get Active Catalogs
        // =========================================

        [HttpGet]
        public async Task<IActionResult>
            GetActiveCatalogs([FromQuery] Guid? partnerId)
        {
            var result =
                await _catalogService
                    .GetActiveCatalogsAsync(partnerId);

            return Ok(result);
        }

        // =========================================
        // Get Catalog By Id
        // =========================================

        [HttpGet("{id:guid}")]
        public async Task<IActionResult>
            GetCatalogById(Guid id)
        {
            var result =
                await _catalogService
                    .GetCatalogByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
