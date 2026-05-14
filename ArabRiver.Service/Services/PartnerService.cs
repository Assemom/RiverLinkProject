using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using ArabRiver.Service.DTOs.Partner;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Responses;
using AutoMapper;

namespace ArabRiver.Service.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly ICatalogRepository _catalogRepository;
        private readonly IMapper _mapper;

        public PartnerService(
            IPartnerRepository partnerRepository,
            ICatalogRepository catalogRepository,
            IMapper mapper)
        {
            _partnerRepository = partnerRepository;
            _catalogRepository = catalogRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<PartnerResponseDto>>>
            GetAllPartnersAsync()
        {
            var partners = await _partnerRepository.GetAllAsync();

            var mappedPartners = _mapper
                .Map<IEnumerable<PartnerResponseDto>>(partners);

            return new ApiResponse<IEnumerable<PartnerResponseDto>>(
                true,
                "Partners retrieved successfully",
                mappedPartners);
        }

        public async Task<ApiResponse<string>>
            CreatePartnerAsync(CreatePartnerDto dto)
        {
            var existingPartner = await _partnerRepository
                .GetByNameAsync(dto.Name);

            if (existingPartner is not null)
            {
                return new ApiResponse<string>(
                    false,
                    "Partner already exists");
            }

            var partner = new Partner
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _partnerRepository.AddAsync(partner);

            await _partnerRepository.SaveChangesAsync();

            return new ApiResponse<string>(
                true,
                "Partner created successfully");
        }

        public async Task<ApiResponse<string>>
            DeletePartnerAsync(Guid id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);

            if (partner is null)
            {
                return new ApiResponse<string>(
                    false,
                    "Partner not found");
            }

            var isUsed = await _catalogRepository
                .ExistsByPartnerIdAsync(id);

            if (isUsed)
            {
                return new ApiResponse<string>(
                    false,
                    "Partner is in use. Update catalogs first");
            }

            _partnerRepository.Delete(partner);

            await _partnerRepository.SaveChangesAsync();

            return new ApiResponse<string>(
                true,
                "Partner deleted successfully");
        }
    }
}
