using ArabRiver.Service.DTOs.Lead;
using ArabRiver.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ArabRiver.Api.Controllers
{
    [ApiController]
    [Route("api/leads")]
    [EnableRateLimiting("LeadPolicy")]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService
            _leadService;

        private readonly IOutsideEgyptVisitorService
            _outsideEgyptVisitorService;

        public LeadController(
            ILeadService leadService,
            IOutsideEgyptVisitorService
                outsideEgyptVisitorService)
        {
            _leadService = leadService;

            _outsideEgyptVisitorService =
                outsideEgyptVisitorService;
        }

        // =========================================
        // Create Lead + Get Download URL
        // =========================================

        [HttpPost]
        public async Task<IActionResult>
            CreateLead(CreateLeadDto dto)
        {
            // Get User IP

            var ipAddress =
                HttpContext
                    .Connection
                    .RemoteIpAddress?
                    .ToString();

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = "Unknown";
            }

            // Create lead

            var result =
                await _leadService
                    .CreateLeadAsync(
                        dto,
                        ipAddress);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // =========================================
        // Outside Egypt Visitor
        // =========================================

        [HttpPost("/api/lead/outside-egypt")]
        [EnableRateLimiting("LeadPolicy")]
        public async Task<IActionResult>
            CreateOutsideEgyptLead(
                CreateOutsideEgyptLeadDto dto)
        {
            var ipAddress =
                HttpContext
                    .Connection
                    .RemoteIpAddress?
                    .ToString();

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = "Unknown";
            }

            var result =
                await _outsideEgyptVisitorService
                    .CreateVisitorAsync(
                        dto,
                        ipAddress);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // =========================================
        // Get Lead Location
        // =========================================

        [HttpGet("/api/lead/location")]
        public async Task<IActionResult>
            GetLeadLocation()
        {
            var ipAddress =
                HttpContext
                    .Connection
                    .RemoteIpAddress?
                    .ToString();

            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                ipAddress = "Unknown";
            }

            var result =
                await _leadService
                    .GetLeadLocationAsync(ipAddress);

            return Ok(result);
        }
    }
}
