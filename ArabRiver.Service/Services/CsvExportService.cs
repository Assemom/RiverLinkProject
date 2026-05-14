using ArabRiver.Repository.Models;
using ArabRiver.Service.Interfaces;
using System.Text;

namespace ArabRiver.Service.Services
{
    public class CsvExportService : ICsvExportService
    {
        public byte[] ExportLeadsToCsv(
            IEnumerable<Lead> leads)
        {
            var csv = new StringBuilder();

            // Header

            csv.AppendLine(
                "FirstName," +
                "PhoneNumber," +
                "OrganizationName," +
                "Country," +
                "CountryCode," +
                "IsEgypt," +
                "CatalogName," +
                "CreatedAt");

            // Rows

            foreach (var lead in leads)
            {
                csv.AppendLine(
                    $"{EscapeCsv(lead.FirstName)}," +
                    $"{EscapeCsv(lead.PhoneNumber)}," +
                    $"{EscapeCsv(lead.OrganizationName)}," +
                    $"{EscapeCsv(lead.Country)}," +
                    $"{EscapeCsv(lead.CountryCode)}," +
                    $"{lead.IsEgypt}," +
                    $"{EscapeCsv(lead.CatalogNameSnapshot)}," +
                    $"{lead.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            }

            return Encoding.UTF8.GetBytes(
                csv.ToString());
        }

        public byte[] ExportOutsideEgyptVisitorsToCsv(
            IEnumerable<OutsideEgyptVisitor> visitors)
        {
            var csv = new StringBuilder();

            csv.AppendLine(
                "Name," +
                "OrganizationName," +
                "Country," +
                "CountryCode," +
                "CreatedAt");

            foreach (var visitor in visitors)
            {
                csv.AppendLine(
                    $"{EscapeCsv(visitor.Name)}," +
                    $"{EscapeCsv(visitor.OrganizationName)}," +
                    $"{EscapeCsv(visitor.Country)}," +
                    $"{EscapeCsv(visitor.CountryCode)}," +
                    $"{visitor.CreatedAt:yyyy-MM-dd HH:mm:ss}");
            }

            return Encoding.UTF8.GetBytes(
                csv.ToString());
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            // Escape quotes

            value = value.Replace("\"", "\"\"");

            // Wrap in quotes if needed

            if (value.Contains(",")
                || value.Contains("\"")
                || value.Contains("\n"))
            {
                value = $"\"{value}\"";
            }

            return value;
        }
    }
}
