using ArabRiver.Service.DTOs.Admin;
using ArabRiver.Service.DTOs.Auth;
using ArabRiver.Service.Responses;

namespace ArabRiver.Service.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>>
            LoginAsync(LoginDto dto);

        Task<ApiResponse<AdminResponseDto>>
            GetCurrentAdminAsync(Guid adminId);
    }
}
