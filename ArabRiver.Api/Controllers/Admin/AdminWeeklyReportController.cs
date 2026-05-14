using ArabRiver.Api.Settings;
using ArabRiver.Api.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ArabRiver.Api.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/reports")]
    public class AdminWeeklyReportController : ControllerBase
    {
        private const string TriggerHeader = "X-Report-Token";

        private readonly WeeklyReportSettings _settings;

        private readonly IBackgroundJobClient _backgroundJobClient;

        public AdminWeeklyReportController(
            IOptions<WeeklyReportSettings> settings,
            IBackgroundJobClient backgroundJobClient)
        {
            _settings = settings.Value;

            _backgroundJobClient = backgroundJobClient;
        }

        [HttpPost("weekly/trigger")]
        [AllowAnonymous]
        public IActionResult TriggerWeeklyReport()
        {
            if (string.IsNullOrWhiteSpace(_settings.TriggerToken))
            {
                return StatusCode(503, "Trigger token is not configured.");
            }

            if (!Request.Headers.TryGetValue(TriggerHeader, out var token)
                || !string.Equals(token, _settings.TriggerToken, StringComparison.Ordinal))
            {
                return Unauthorized();
            }

            _backgroundJobClient.Enqueue<WeeklyReportsJob>(
                job => job.SendReportsAsync());

            return Ok(new { message = "Weekly report job queued." });
        }
    }
}
