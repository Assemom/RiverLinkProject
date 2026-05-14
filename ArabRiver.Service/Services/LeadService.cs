using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using ArabRiver.Service.DTOs.Lead;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;
using AutoMapper;

namespace ArabRiver.Service.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;

        private readonly ICatalogRepository _catalogRepository;

        private readonly IGeolocationService _geolocationService;

        private readonly ICsvExportService _csvExportService;

        private readonly IMapper _mapper;

        public LeadService(
            ILeadRepository leadRepository,
            ICatalogRepository catalogRepository,
            IGeolocationService geolocationService,
            ICsvExportService csvExportService,
            IMapper mapper)
        {
            _leadRepository = leadRepository;

            _catalogRepository = catalogRepository;

            _geolocationService = geolocationService;

            _csvExportService = csvExportService;

            _mapper = mapper;
        }

        public async Task<ApiResponse<LeadDownloadResponseDto>>
            CreateLeadAsync(
                CreateLeadDto dto,
                string ipAddress)
        {
            // Check catalog exists

            var catalog =
                await _catalogRepository
                    .GetByIdAsync(dto.CatalogId);

            if (catalog is null || !catalog.IsActive)
            {
                return new ApiResponse<LeadDownloadResponseDto>(
                    false,
                    "Catalog not found");
            }

            // Get visitor location

            var location =
                await _geolocationService
                    .GetLocationAsync(ipAddress);

            // Create lead

            var lead =
                _mapper.Map<Lead>(dto);

            lead.Id = Guid.NewGuid();

            lead.Country = location.Country;

            lead.CountryCode = location.CountryCode;

            lead.IsEgypt = location.IsEgypt;

            lead.IpAddress = ipAddress;

            lead.CatalogNameSnapshot =
                catalog.Name;

            lead.CreatedAt = DateTime.UtcNow;

            // Save lead

            await _leadRepository.AddAsync(lead);

            await _leadRepository.SaveChangesAsync();

            // Return download URL

            var response =
                new LeadDownloadResponseDto
                {
                    DownloadUrl =
                        catalog.GoogleDriveLink
                };

            return new ApiResponse<LeadDownloadResponseDto>(
                true,
                "Lead created successfully",
                response);
        }

        public async Task<ApiResponse<LeadLocationResponseDto>>
            GetLeadLocationAsync(
                string ipAddress)
        {
            var location =
                await _geolocationService
                    .GetLocationAsync(ipAddress);

            var response =
                new LeadLocationResponseDto
                {
                    IpAddress = ipAddress,
                    Country = location.Country,
                    CountryCode = location.CountryCode,
                    IsEgypt = location.IsEgypt
                };

            return new ApiResponse<LeadLocationResponseDto>(
                true,
                "Location retrieved successfully",
                response);
        }

        public async Task<PagedResponse<LeadResponseDto>>
            GetAllLeadsAsync(
                PaginationParameters parameters)
        {
            var leads =
                await _leadRepository.GetAllAsync();

            var totalCount = leads.Count();

            var pagedLeads = leads
                .Skip((parameters.PageNumber - 1)
                    * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var mappedLeads =
                _mapper.Map<IEnumerable<LeadResponseDto>>(
                    pagedLeads);

            return new PagedResponse<LeadResponseDto>(
                mappedLeads,
                parameters.PageNumber,
                parameters.PageSize,
                totalCount);
        }

        public async Task<byte[]>
            ExportLeadsToCsvAsync()
        {
            var leads =
                await _leadRepository.GetAllAsync();

            return _csvExportService
                .ExportLeadsToCsv(leads);
        }

        public async Task<byte[]>
            ExportLeadsToCsvAsync(
                DateTime startUtc,
                DateTime endUtc)
        {
            var leads =
                await _leadRepository
                    .GetByDateRangeAsync(
                        startUtc,
                        endUtc);

            return _csvExportService
                .ExportLeadsToCsv(leads);
        }
    }
}
