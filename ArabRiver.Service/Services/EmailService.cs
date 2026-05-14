using ArabRiver.Service.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ArabRiver.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(
            IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendContactNotificationAsync(
            string name,
            string email,
            string message)
        {
            var mail = new MimeMessage();

            mail.From.Add(
                new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

            mail.To.Add(
                MailboxAddress.Parse(
                    _emailSettings.SenderEmail));

            mail.Subject = "New Contact Message";

            mail.Body =
                new TextPart("html")
                {
                    Text =
                        $"""
                    <h2>New Contact Message</h2>

                    <p>
                        <strong>Name:</strong>
                        {name}
                    </p>

                    <p>
                        <strong>Email:</strong>
                        {email}
                    </p>

                    <p>
                        <strong>Message:</strong>
                    </p>

                    <p>
                        {message}
                    </p>
                    """
                };

            await SendEmailAsync(mail);
        }

        public async Task SendWeeklyReportAsync(
            string subject,
            string htmlBody,
            IEnumerable<string> recipients,
            string attachmentFileName,
            byte[] attachmentContent)
        {
            var mail = new MimeMessage();

            mail.From.Add(
                new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

            foreach (var recipient in recipients
                .Where(recipient =>
                    !string.IsNullOrWhiteSpace(recipient)))
            {
                mail.To.Add(
                    MailboxAddress.Parse(recipient));
            }

            if (!mail.To.Any())
            {
                return;
            }

            mail.Subject = subject;

            var builder =
                new BodyBuilder
                {
                    HtmlBody = htmlBody
                };

            builder.Attachments.Add(
                attachmentFileName,
                attachmentContent,
                new ContentType("text", "csv"));

            mail.Body = builder.ToMessageBody();

            await SendEmailAsync(mail);
        }

        public async Task SendOutsideEgyptLeadNotificationAsync(
            string name,
            string? organizationName,
            string country,
            string countryCode)
        {
            var mail = new MimeMessage();

            mail.From.Add(
                new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

            mail.To.Add(
                MailboxAddress.Parse(
                    _emailSettings.SenderEmail));

            mail.Subject =
                "New Outside Egypt Visitor";

            var organizationValue =
                string.IsNullOrWhiteSpace(organizationName)
                ? "Not provided"
                : organizationName;

            mail.Body =
                new TextPart("html")
                {
                    Text =
                        $"""
                    <h2>Outside Egypt Visitor</h2>

                    <p>
                        <strong>Name:</strong>
                        {name}
                    </p>

                    <p>
                        <strong>Clinic/Hospital:</strong>
                        {organizationValue}
                    </p>

                    <p>
                        <strong>Country:</strong>
                        {country} ({countryCode})
                    </p>
                    """
                };

            await SendEmailAsync(mail);
        }

        public async Task SendAutoReplyAsync(
            string email,
            string name)
        {
            var mail = new MimeMessage();

            mail.From.Add(
                new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail));

            mail.To.Add(
                MailboxAddress.Parse(email));

            mail.Subject = "We received your message";

            mail.Body =
                new TextPart("html")
                {
                    Text =
                        $"""
                    <p>
                        Dear {name},
                    </p>

                    <p>
                        We received your message
                        and will contact you soon.
                    </p>

                    <p>
                        Regards,
                        Arab River
                    </p>
                    """
                };

            await SendEmailAsync(mail);
        }

        private async Task SendEmailAsync(
            MimeMessage mail)
        {
            using var smtp =
                new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(
                _emailSettings.SmtpServer,
                _emailSettings.Port,
                false);

            await smtp.AuthenticateAsync(
                _emailSettings.Username,
                _emailSettings.Password);

            await smtp.SendAsync(mail);

            await smtp.DisconnectAsync(true);
        }
    }
}
