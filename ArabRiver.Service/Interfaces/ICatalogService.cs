using ArabRiver.Service.DTOs.Catalog;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;

namespace ArabRiver.Service.Interfaces
{
    public interface ICatalogService
    {
        Task<ApiResponse<IEnumerable<CatalogResponseDto>>>
            GetActiveCatalogsAsync(Guid? partnerId);

        Task<PagedResponse<CatalogResponseDto>>
            GetAllCatalogsAsync(
                PaginationParameters parameters,
                Guid? partnerId);

        Task<ApiResponse<CatalogResponseDto>>
            GetCatalogByIdAsync(Guid id);

        Task<ApiResponse<string>>
            CreateCatalogAsync(CreateCatalogDto dto);

        Task<ApiResponse<string>>
            UpdateCatalogAsync(
                Guid id,
                UpdateCatalogDto dto);

        Task<ApiResponse<string>>
            UpdateCatalogStatusAsync(
                Guid id,
                bool isActive);
    }
}
