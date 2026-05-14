using ArabRiver.Repository.Models;

namespace ArabRiver.Service.Interfaces
{
    public interface ICsvExportService
    {
        byte[] ExportLeadsToCsv(
            IEnumerable<Lead> leads);

        byte[] ExportOutsideEgyptVisitorsToCsv(
            IEnumerable<OutsideEgyptVisitor> visitors);
    }
}
