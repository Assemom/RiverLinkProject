using ArabRiver.Api.Settings;
using ArabRiver.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace ArabRiver.Api.Jobs
{
    public class WeeklyReportsJob
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly WeeklyReportSettings _settings;

        private readonly ILogger<WeeklyReportsJob> _logger;

        public WeeklyReportsJob(
            IServiceProvider serviceProvider,
            IOptions<WeeklyReportSettings> settings,
            ILogger<WeeklyReportsJob> logger)
        {
            _serviceProvider = serviceProvider;

            _settings = settings.Value;

            _logger = logger;
        }

        public async Task SendReportsAsync()
        {
            if (_settings.AdminEmails.Length == 0)
            {
                _logger.LogWarning(
                    "Weekly reports skipped because no admin emails are configured.");

                return;
            }

            var timeZone = ResolveTimeZoneWithLogging();

            var nowLocal =
                TimeZoneInfo.ConvertTime(
                    DateTime.UtcNow,
                    timeZone);

            var startLocal =
                nowLocal.AddDays(-7);

            var startUtc =
                TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.SpecifyKind(
                        startLocal,
                        DateTimeKind.Unspecified),
                    timeZone);

            var endUtc =
                TimeZoneInfo.ConvertTimeToUtc(
                    DateTime.SpecifyKind(
                        nowLocal,
                        DateTimeKind.Unspecified),
                    timeZone);

            using var scope =
                _serviceProvider.CreateScope();

            var leadService =
                scope.ServiceProvider
                    .GetRequiredService<ILeadService>();

            var visitorService =
                scope.ServiceProvider
                    .GetRequiredService<IOutsideEgyptVisitorService>();

            var emailService =
                scope.ServiceProvider
                    .GetRequiredService<IEmailService>();

            var leadsCsv =
                await leadService
                    .ExportLeadsToCsvAsync(
                        startUtc,
                        endUtc);

            var visitorsCsv =
                await visitorService
                    .ExportVisitorsToCsvAsync(
                        startUtc,
                        endUtc);

            var periodLabel =
                $"{startLocal:yyyy-MM-dd} to {nowLocal:yyyy-MM-dd}";

            var leadsSubject =
                $"Weekly Catalog Download Leads ({periodLabel})";

            var leadsBody =
                $"""
                <p>Attached is the weekly report for catalog download leads.</p>
                <p>Period: {periodLabel} (Egypt time).</p>
                """;

            await emailService
                .SendWeeklyReportAsync(
                    leadsSubject,
                    leadsBody,
                    _settings.AdminEmails,
                    $"catalog-leads-{nowLocal:yyyyMMdd}.csv",
                    leadsCsv);

            var visitorsSubject =
                $"Weekly Outside Egypt Visitors ({periodLabel})";

            var visitorsBody =
                $"""
                <p>Attached is the weekly report for outside Egypt visitors.</p>
                <p>Period: {periodLabel} (Egypt time).</p>
                """;

            await emailService
                .SendWeeklyReportAsync(
                    visitorsSubject,
                    visitorsBody,
                    _settings.AdminEmails,
                    $"outside-egypt-visitors-{nowLocal:yyyyMMdd}.csv",
                    visitorsCsv);
        }

        public static TimeZoneInfo ResolveTimeZone(string timeZoneId)
        {
            try
            {
                return TimeZoneInfo
                    .FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.Utc;
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.Utc;
            }
        }

        private TimeZoneInfo ResolveTimeZoneWithLogging()
        {
            try
            {
                return TimeZoneInfo
                    .FindSystemTimeZoneById(
                        _settings.TimeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                _logger.LogWarning(
                    "Time zone {TimeZoneId} was not found. Falling back to UTC.",
                    _settings.TimeZoneId);

                return TimeZoneInfo.Utc;
            }
            catch (InvalidTimeZoneException)
            {
                _logger.LogWarning(
                    "Time zone {TimeZoneId} is invalid. Falling back to UTC.",
                    _settings.TimeZoneId);

                return TimeZoneInfo.Utc;
            }
        }
    }
}
