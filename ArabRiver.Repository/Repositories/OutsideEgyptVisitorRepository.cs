using ArabRiver.Repository.Data;
using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Repository.Repositories
{
    public class OutsideEgyptVisitorRepository
        : IOutsideEgyptVisitorRepository
    {
        private readonly ApplicationDbContext _context;

        public OutsideEgyptVisitorRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OutsideEgyptVisitor>> GetAllAsync()
        {
            return await _context.OutsideEgyptVisitors
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<OutsideEgyptVisitor>> GetByDateRangeAsync(
            DateTime startUtc,
            DateTime endUtc)
        {
            return await _context.OutsideEgyptVisitors
                .Where(x => x.CreatedAt >= startUtc
                    && x.CreatedAt < endUtc)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(OutsideEgyptVisitor visitor)
        {
            await _context.OutsideEgyptVisitors.AddAsync(visitor);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
