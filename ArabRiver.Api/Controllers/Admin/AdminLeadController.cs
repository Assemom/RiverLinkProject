using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArabRiver.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/leads")]
    [Authorize]
    public class AdminLeadController : ControllerBase
    {
        private readonly ILeadService
            _leadService;

        public AdminLeadController(
            ILeadService leadService)
        {
            _leadService = leadService;
        }

        // =========================================
        // Get All Leads
        // =========================================

        [HttpGet]
        public async Task<IActionResult>
            GetAllLeads(
                [FromQuery]
            PaginationParameters parameters)
        {
            var result =
                await _leadService
                    .GetAllLeadsAsync(
                        parameters);

            return Ok(result);
        }

        // =========================================
        // Export Leads CSV
        // =========================================

        [HttpGet("export")]
        public async Task<IActionResult>
            ExportLeads()
        {
            var csvBytes =
                await _leadService
                    .ExportLeadsToCsvAsync();

            return File(
                csvBytes,
                "text/csv",
                $"leads-{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
        }
    }
}
