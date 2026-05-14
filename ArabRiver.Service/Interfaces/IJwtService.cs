using ArabRiver.Repository.Models;

namespace ArabRiver.Service.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Admin admin);
    }
}
