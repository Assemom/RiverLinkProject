using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using ArabRiver.Service.DTOs.Lead;
using ArabRiver.Service.DTOs.OutsideEgyptVisitor;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;
using AutoMapper;

namespace ArabRiver.Service.Services
{
    public class OutsideEgyptVisitorService
        : IOutsideEgyptVisitorService
    {
        private readonly IOutsideEgyptVisitorRepository
            _visitorRepository;

        private readonly IGeolocationService
            _geolocationService;

        private readonly IEmailService _emailService;

        private readonly ICsvExportService _csvExportService;

        private readonly IMapper _mapper;

        public OutsideEgyptVisitorService(
            IOutsideEgyptVisitorRepository visitorRepository,
            IGeolocationService geolocationService,
            IEmailService emailService,
            ICsvExportService csvExportService,
            IMapper mapper)
        {
            _visitorRepository = visitorRepository;

            _geolocationService = geolocationService;

            _emailService = emailService;

            _csvExportService = csvExportService;

            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> CreateVisitorAsync(
            CreateOutsideEgyptLeadDto dto,
            string ipAddress)
        {
            var location =
                await _geolocationService
                    .GetLocationAsync(ipAddress);

            if (location.IsEgypt)
            {
                return new ApiResponse<string>(
                    false,
                    "This endpoint is only for outside Egypt visitors");
            }

            var visitor = new OutsideEgyptVisitor
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                OrganizationName = dto.OrganizationName,
                Country = location.Country,
                CountryCode = location.CountryCode,
                IpAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _visitorRepository.AddAsync(visitor);

            await _visitorRepository.SaveChangesAsync();

            return new ApiResponse<string>(
                true,
                "Outside Egypt visitor sent successfully");
        }

        public async Task<PagedResponse<OutsideEgyptVisitorResponseDto>>
            GetAllVisitorsAsync(
                PaginationParameters parameters)
        {
            var visitors =
                await _visitorRepository.GetAllAsync();

            var totalCount = visitors.Count();

            var pagedVisitors = visitors
                .Skip((parameters.PageNumber - 1)
                    * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var mappedVisitors =
                _mapper.Map<
                    IEnumerable<OutsideEgyptVisitorResponseDto>>(
                    pagedVisitors);

            return new PagedResponse<
                OutsideEgyptVisitorResponseDto>(
                mappedVisitors,
                parameters.PageNumber,
                parameters.PageSize,
                totalCount);
        }

        public async Task<byte[]>
            ExportVisitorsToCsvAsync(
                DateTime startUtc,
                DateTime endUtc)
        {
            var visitors =
                await _visitorRepository
                    .GetByDateRangeAsync(
                        startUtc,
                        endUtc);

            return _csvExportService
                .ExportOutsideEgyptVisitorsToCsv(visitors);
        }
    }
}
