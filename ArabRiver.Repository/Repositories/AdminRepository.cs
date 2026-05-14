using ArabRiver.Repository.Data;
using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Repository.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Admin?> GetByIdAsync(Guid id)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _context.Admins
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
        }

        public void Update(Admin admin)
        {
            _context.Admins.Update(admin);
        }

        public void Delete(Admin admin)
        {
            _context.Admins.Remove(admin);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
