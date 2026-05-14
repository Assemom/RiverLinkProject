using ArabRiver.Repository.Interfaces;
using ArabRiver.Service.DTOs.Admin;
using ArabRiver.Service.DTOs.Auth;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Responses;
using AutoMapper;

namespace ArabRiver.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAdminRepository
            _adminRepository;

        private readonly IJwtService
            _jwtService;

        private readonly IMapper
            _mapper;

        public AuthService(
            IAdminRepository adminRepository,
            IJwtService jwtService,
            IMapper mapper)
        {
            _adminRepository = adminRepository;

            _jwtService = jwtService;

            _mapper = mapper;
        }

        public async Task<ApiResponse<LoginResponseDto>>
            LoginAsync(LoginDto dto)
        {
            // Find admin

            var admin =
                await _adminRepository
                    .GetByEmailAsync(dto.Email);

            if (admin is null)
            {
                return new ApiResponse<LoginResponseDto>(
                    false,
                    "Invalid email or password");
            }

            // Verify password

            var isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    admin.PasswordHash);

            if (!isPasswordValid)
            {
                return new ApiResponse<LoginResponseDto>(
                    false,
                    "Invalid email or password");
            }

            // Generate JWT

            var token =
                _jwtService.GenerateToken(admin);

            var response =
                new LoginResponseDto
                {
                    Token = token,

                    Expiration =
                        DateTime.UtcNow.AddHours(12)
                };

            return new ApiResponse<LoginResponseDto>(
                true,
                "Login successful",
                response);
        }

        public async Task<ApiResponse<AdminResponseDto>>
            GetCurrentAdminAsync(Guid adminId)
        {
            var admin =
                await _adminRepository
                    .GetByIdAsync(adminId);

            if (admin is null)
            {
                return new ApiResponse<AdminResponseDto>(
                    false,
                    "Admin not found");
            }

            var mappedAdmin =
                _mapper.Map<AdminResponseDto>(
                    admin);

            return new ApiResponse<AdminResponseDto>(
                true,
                "Admin retrieved successfully",
                mappedAdmin);
        }
    }
}
