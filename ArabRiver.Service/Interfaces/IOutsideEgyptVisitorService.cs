using ArabRiver.Service.DTOs.Lead;
using ArabRiver.Service.DTOs.OutsideEgyptVisitor;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;

namespace ArabRiver.Service.Interfaces
{
    public interface IOutsideEgyptVisitorService
    {
        Task<ApiResponse<string>> CreateVisitorAsync(
            CreateOutsideEgyptLeadDto dto,
            string ipAddress);

        Task<PagedResponse<OutsideEgyptVisitorResponseDto>>
            GetAllVisitorsAsync(PaginationParameters parameters);

        Task<byte[]>
            ExportVisitorsToCsvAsync(
                DateTime startUtc,
                DateTime endUtc);
    }
}
