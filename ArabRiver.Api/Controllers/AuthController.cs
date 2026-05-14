using ArabRiver.Service.DTOs.Auth;
using ArabRiver.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ArabRiver.Api.Controllers
{
    [ApiController]
    [Route("api/admin/auth")]
    [EnableRateLimiting("LoginPolicy")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService
            _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult>
            Login(LoginDto dto)
        {
            var result =
                await _authService
                    .LoginAsync(dto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult>
            GetCurrentAdmin()
        {
            var adminId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Unauthorized();
            }

            var result =
                await _authService
                    .GetCurrentAdminAsync(
                        Guid.Parse(adminId));

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
