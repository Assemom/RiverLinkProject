namespace ArabRiver.Api.Settings
{
    public class WeeklyReportSettings
    {
        public string[] AdminEmails { get; set; } = Array.Empty<string>();

        public string TriggerToken { get; set; } = string.Empty;

        public string TimeZoneId { get; set; } = "Egypt Standard Time";

        public DayOfWeek RunDayOfWeek { get; set; } = DayOfWeek.Monday;

        public TimeSpan RunTime { get; set; } = new(9, 0, 0);
    }
}
