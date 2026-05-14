using ArabRiver.Repository.Data;
using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Repository.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly ApplicationDbContext _context;

        public LeadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lead>> GetAllAsync()
        {
            return await _context.Leads
                .Include(x => x.Catalog)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> GetByDateRangeAsync(
            DateTime startUtc,
            DateTime endUtc)
        {
            return await _context.Leads
                .Include(x => x.Catalog)
                .Where(x => x.CreatedAt >= startUtc
                    && x.CreatedAt < endUtc)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Lead?> GetByIdAsync(Guid id)
        {
            return await _context.Leads
                .Include(x => x.Catalog)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Lead lead)
        {
            await _context.Leads.AddAsync(lead);
        }

        public void Update(Lead lead)
        {
            _context.Leads.Update(lead);
        }

        public void Delete(Lead lead)
        {
            _context.Leads.Remove(lead);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
