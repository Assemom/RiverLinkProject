using ArabRiver.Repository.Data;
using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ArabRiver.Repository.Repositories
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactMessage>> GetAllAsync()
        {
            return await _context.ContactMessages
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ContactMessage?> GetByIdAsync(Guid id)
        {
            return await _context.ContactMessages
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(ContactMessage contactMessage)
        {
            await _context.ContactMessages.AddAsync(contactMessage);
        }

        public void Update(ContactMessage contactMessage)
        {
            _context.ContactMessages.Update(contactMessage);
        }

        public void Delete(ContactMessage contactMessage)
        {
            _context.ContactMessages.Remove(contactMessage);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
