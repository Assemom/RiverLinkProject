using ArabRiver.Repository.Models;

namespace ArabRiver.Repository.Interfaces
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<Partner>> GetAllAsync();

        Task<Partner?> GetByIdAsync(Guid id);

        Task<Partner?> GetByNameAsync(string name);

        Task AddAsync(Partner partner);

        void Delete(Partner partner);

        Task SaveChangesAsync();
    }
}
