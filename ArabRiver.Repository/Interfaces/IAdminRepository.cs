using ArabRiver.Repository.Models;

namespace ArabRiver.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin?> GetByEmailAsync(string email);

        Task<Admin?> GetByIdAsync(Guid id);

        Task<IEnumerable<Admin>> GetAllAsync();

        Task AddAsync(Admin admin);

        void Update(Admin admin);

        void Delete(Admin admin);

        Task SaveChangesAsync();
    }
}
