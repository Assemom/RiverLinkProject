using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArabRiver.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/outside-egypt-visitors")]
    [Authorize]
    public class AdminOutsideEgyptVisitorController
        : ControllerBase
    {
        private readonly IOutsideEgyptVisitorService
            _outsideEgyptVisitorService;

        public AdminOutsideEgyptVisitorController(
            IOutsideEgyptVisitorService
                outsideEgyptVisitorService)
        {
            _outsideEgyptVisitorService =
                outsideEgyptVisitorService;
        }

        // =========================================
        // Get All Outside Egypt Visitors
        // =========================================

        [HttpGet]
        public async Task<IActionResult>
            GetAllVisitors(
                [FromQuery]
            PaginationParameters parameters)
        {
            var result =
                await _outsideEgyptVisitorService
                    .GetAllVisitorsAsync(
                        parameters);

            return Ok(result);
        }
    }
}
