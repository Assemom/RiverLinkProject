namespace ArabRiver.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendContactNotificationAsync(
            string name,
            string email,
            string message);

        Task SendAutoReplyAsync(
            string email,
            string name);

        Task SendOutsideEgyptLeadNotificationAsync(
            string name,
            string? organizationName,
            string country,
            string countryCode);

        Task SendWeeklyReportAsync(
            string subject,
            string htmlBody,
            IEnumerable<string> recipients,
            string attachmentFileName,
            byte[] attachmentContent);
    }
}
