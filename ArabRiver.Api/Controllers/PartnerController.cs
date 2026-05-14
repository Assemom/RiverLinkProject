using ArabRiver.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArabRiver.Api.Controllers
{
    [ApiController]
    [Route("api/partners")]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnerController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPartners()
        {
            var result = await _partnerService.GetAllPartnersAsync();

            return Ok(result);
        }
    }
}
