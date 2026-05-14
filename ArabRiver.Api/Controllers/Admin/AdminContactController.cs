using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArabRiver.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/contact-messages")]
    [Authorize]
    public class AdminContactController : ControllerBase
    {
        private readonly IContactService
            _contactService;

        public AdminContactController(
            IContactService contactService)
        {
            _contactService = contactService;
        }

        // =========================================
        // Get All Contact Messages
        // =========================================

        [HttpGet]
        public async Task<IActionResult>
            GetAllMessages(
                [FromQuery]
            PaginationParameters parameters)
        {
            var result =
                await _contactService
                    .GetAllMessagesAsync(
                        parameters);

            return Ok(result);
        }

        // =========================================
        // Mark Message As Read
        // =========================================

        [HttpPatch("{id:guid}/read")]
        public async Task<IActionResult>
            MarkAsRead(Guid id)
        {
            var result =
                await _contactService
                    .MarkAsReadAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
