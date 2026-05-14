using ArabRiver.Repository.Models;

namespace ArabRiver.Repository.Interfaces
{
    public interface IOutsideEgyptVisitorRepository
    {
        Task<IEnumerable<OutsideEgyptVisitor>> GetAllAsync();

        Task<IEnumerable<OutsideEgyptVisitor>> GetByDateRangeAsync(
            DateTime startUtc,
            DateTime endUtc);

        Task AddAsync(OutsideEgyptVisitor visitor);

        Task SaveChangesAsync();
    }
}
