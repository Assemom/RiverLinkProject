using ArabRiver.Service.DTOs.Lead;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;

namespace ArabRiver.Service.Interfaces
{
    public interface ILeadService
    {
        Task<ApiResponse<LeadDownloadResponseDto>>
            CreateLeadAsync(
                CreateLeadDto dto,
                string ipAddress);

        Task<ApiResponse<LeadLocationResponseDto>>
            GetLeadLocationAsync(
                string ipAddress);

        Task<PagedResponse<LeadResponseDto>>
            GetAllLeadsAsync(
                PaginationParameters parameters);

        Task<byte[]>
            ExportLeadsToCsvAsync();

        Task<byte[]>
            ExportLeadsToCsvAsync(
                DateTime startUtc,
                DateTime endUtc);
    }
}
