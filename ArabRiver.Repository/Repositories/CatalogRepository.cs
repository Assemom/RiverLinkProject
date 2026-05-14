using ArabRiver.Repository.Data;
using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Repository.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly ApplicationDbContext _context;

        public CatalogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Catalog>> GetAllAsync()
        {
            return await _context.Catalogs
                .Include(x => x.Partner)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<Catalog>> GetActiveAsync()
        {
            return await _context.Catalogs
                .Include(x => x.Partner)
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Catalog?> GetByIdAsync(Guid id)
        {
            return await _context.Catalogs
                .Include(x => x.Partner)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsByPartnerIdAsync(Guid partnerId)
        {
            return await _context.Catalogs
                .AnyAsync(x => x.PartnerId == partnerId);
        }

        public async Task AddAsync(Catalog catalog)
        {
            await _context.Catalogs.AddAsync(catalog);
        }

        public void Update(Catalog catalog)
        {
            _context.Catalogs.Update(catalog);
        }

        public void Delete(Catalog catalog)
        {
            _context.Catalogs.Remove(catalog);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
