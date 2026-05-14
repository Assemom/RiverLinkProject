using ArabRiver.Repository.Models;
using ArabRiver.Service.Helpers;
using ArabRiver.Service.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArabRiver.Service.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(Admin admin)
        {
            // Claims

            var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                admin.Id.ToString()),

            new Claim(
                ClaimTypes.Email,
                admin.Email),

            new Claim(
                ClaimTypes.Role,
                admin.Role),

            new Claim(
                ClaimTypes.Name,
                admin.Name)
        };

            // Signing key

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _jwtSettings.Key));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            // Token

            var jwtToken =
                new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,

                    audience: _jwtSettings.Audience,

                    claims: claims,

                    expires:
                        DateTime.UtcNow.AddMinutes(
                            _jwtSettings
                                .DurationInMinutes),

                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(jwtToken);
        }
    }
}
