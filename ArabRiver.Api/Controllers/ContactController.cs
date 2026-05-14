using ArabRiver.Service.DTOs.Contact;
using ArabRiver.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ArabRiver.Api.Controllers
{
    [ApiController]
    [Route("api/contact")]
    [EnableRateLimiting("ContactPolicy")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService
            _contactService;

        public ContactController(
            IContactService contactService)
        {
            _contactService = contactService;
        }

        // =========================================
        // Send Contact Message
        // =========================================

        [HttpPost]
        public async Task<IActionResult>
            CreateMessage(
                CreateContactMessageDto dto)
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

            // Create message

            var result =
                await _contactService
                    .CreateMessageAsync(
                        dto,
                        ipAddress);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
