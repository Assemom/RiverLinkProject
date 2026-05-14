using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using ArabRiver.Service.DTOs.Catalog;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;
using AutoMapper;

namespace ArabRiver.Service.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IPartnerRepository _partnerRepository;

        private readonly IMapper _mapper;

        public CatalogService(
            ICatalogRepository catalogRepository,
            IPartnerRepository partnerRepository,
            IMapper mapper)
        {
            _catalogRepository = catalogRepository;
            _partnerRepository = partnerRepository;

            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<CatalogResponseDto>>>
            GetActiveCatalogsAsync(Guid? partnerId)
        {
            var catalogs =
                await _catalogRepository.GetActiveAsync();

            if (partnerId.HasValue)
            {
                catalogs = catalogs
                    .Where(x => x.PartnerId == partnerId);
            }

            var mappedCatalogs =
                _mapper.Map<IEnumerable<CatalogResponseDto>>(
                    catalogs);

            return new ApiResponse<IEnumerable<CatalogResponseDto>>(
                true,
                "Catalogs retrieved successfully",
                mappedCatalogs);
        }

        public async Task<PagedResponse<CatalogResponseDto>>
            GetAllCatalogsAsync(
                PaginationParameters parameters,
                Guid? partnerId)
        {
            var catalogs =
                await _catalogRepository.GetAllAsync();

            if (partnerId.HasValue)
            {
                catalogs = catalogs
                    .Where(x => x.PartnerId == partnerId);
            }

            var totalCount = catalogs.Count();

            var pagedCatalogs = catalogs
                .Skip((parameters.PageNumber - 1)
                    * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var mappedCatalogs =
                _mapper.Map<IEnumerable<CatalogResponseDto>>(
                    pagedCatalogs);

            return new PagedResponse<CatalogResponseDto>(
                mappedCatalogs,
                parameters.PageNumber,
                parameters.PageSize,
                totalCount);
        }

        public async Task<ApiResponse<CatalogResponseDto>>
            GetCatalogByIdAsync(Guid id)
        {
            var catalog =
                await _catalogRepository.GetByIdAsync(id);

            if (catalog is null)
            {
                return new ApiResponse<CatalogResponseDto>(
                    false,
                    "Catalog not found");
            }

            var mappedCatalog =
                _mapper.Map<CatalogResponseDto>(catalog);

            return new ApiResponse<CatalogResponseDto>(
                true,
                "Catalog retrieved successfully",
                mappedCatalog);
        }

        public async Task<ApiResponse<string>>
            CreateCatalogAsync(CreateCatalogDto dto)
        {
            if (dto.PartnerId.HasValue)
            {
                var partner = await _partnerRepository
                    .GetByIdAsync(dto.PartnerId.Value);

                if (partner is null)
                {
                    return new ApiResponse<string>(
                        false,
                        "Partner not found");
                }
            }

            var catalog =
                _mapper.Map<Catalog>(dto);

            catalog.Id = Guid.NewGuid();

            catalog.IsActive = true;

            catalog.CreatedAt = DateTime.UtcNow;

            catalog.UpdatedAt = DateTime.UtcNow;

            await _catalogRepository.AddAsync(catalog);

            await _catalogRepository.SaveChangesAsync();

            return new ApiResponse<string>(
                true,
                "Catalog created successfully");
        }

        public async Task<ApiResponse<string>>
            UpdateCatalogAsync(
                Guid id,
                UpdateCatalogDto dto)
        {
            if (dto.PartnerId.HasValue)
            {
                var partner = await _partnerRepository
                    .GetByIdAsync(dto.PartnerId.Value);

                if (partner is null)
                {
                    return new ApiResponse<string>(
                        false,
                        "Partner not found");
                }
            }

            var catalog =
                await _catalogRepository.GetByIdAsync(id);

            if (catalog is null)
            {
                return new ApiResponse<string>(
                    false,
                    "Catalog not found");
            }

            catalog.Name = dto.Name;

            catalog.Description = dto.Description;

            catalog.GoogleDriveLink =
                dto.GoogleDriveLink;

            catalog.ThumbnailUrl =
                dto.ThumbnailUrl;

            catalog.PartnerId =
                dto.PartnerId;

            catalog.DisplayOrder =
                dto.DisplayOrder;

            catalog.UpdatedAt =
                DateTime.UtcNow;

            _catalogRepository.Update(catalog);

            await _catalogRepository.SaveChangesAsync();

            return new ApiResponse<string>(
                true,
                "Catalog updated successfully");
        }

        public async Task<ApiResponse<string>>
            UpdateCatalogStatusAsync(
                Guid id,
                bool isActive)
        {
            var catalog =
                await _catalogRepository.GetByIdAsync(id);

            if (catalog is null)
            {
                return new ApiResponse<string>(
                    false,
                    "Catalog not found");
            }

            catalog.IsActive = isActive;

            catalog.UpdatedAt = DateTime.UtcNow;

            _catalogRepository.Update(catalog);

            await _catalogRepository.SaveChangesAsync();

            return new ApiResponse<string>(
                true,
                isActive
                ? "Catalog activated successfully"
                : "Catalog deactivated successfully");
        }
    }
}
