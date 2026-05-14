using ArabRiver.Service.DTOs.Partner;
using ArabRiver.Service.Responses;

namespace ArabRiver.Service.Interfaces
{
    public interface IPartnerService
    {
        Task<ApiResponse<IEnumerable<PartnerResponseDto>>> GetAllPartnersAsync();

        Task<ApiResponse<string>> CreatePartnerAsync(CreatePartnerDto dto);

        Task<ApiResponse<string>> DeletePartnerAsync(Guid id);
    }
}
