using ArabRiver.Repository.Models;

namespace ArabRiver.Repository.Interfaces
{
    public interface ILeadRepository
    {
        Task<IEnumerable<Lead>> GetAllAsync();

        Task<IEnumerable<Lead>> GetByDateRangeAsync(
            DateTime startUtc,
            DateTime endUtc);

        Task<Lead?> GetByIdAsync(Guid id);

        Task AddAsync(Lead lead);

        void Update(Lead lead);

        void Delete(Lead lead);

        Task SaveChangesAsync();
    }
}
