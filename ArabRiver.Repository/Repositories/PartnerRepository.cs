using ArabRiver.Repository.Data;
using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Repository.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly ApplicationDbContext _context;

        public PartnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            return await _context.Partners
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Partner?> GetByIdAsync(Guid id)
        {
            return await _context.Partners
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Partner?> GetByNameAsync(string name)
        {
            return await _context.Partners
                .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Partner partner)
        {
            await _context.Partners.AddAsync(partner);
        }

        public void Delete(Partner partner)
        {
            _context.Partners.Remove(partner);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
