using ArabRiver.Api.Models.Partner;
using ArabRiver.Service.DTOs.Partner;
using ArabRiver.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArabRiver.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/partners")]
    [Authorize]
    public class AdminPartnerController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public AdminPartnerController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartner(
            [FromBody] CreatePartnerRequest request)
        {
            var dto = new CreatePartnerDto
            {
                Name = request.Name
            };

            var result = await _partnerService.CreatePartnerAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePartner(Guid id)
        {
            var result = await _partnerService.DeletePartnerAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
