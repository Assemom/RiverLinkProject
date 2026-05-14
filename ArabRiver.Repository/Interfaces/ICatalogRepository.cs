using ArabRiver.Repository.Models;

namespace ArabRiver.Repository.Interfaces
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<Catalog>> GetAllAsync();

        Task<IEnumerable<Catalog>> GetActiveAsync();

        Task<Catalog?> GetByIdAsync(Guid id);

        Task<bool> ExistsByPartnerIdAsync(Guid partnerId);

        Task AddAsync(Catalog catalog);

        void Update(Catalog catalog);

        void Delete(Catalog catalog);

        Task SaveChangesAsync();
    }
}
