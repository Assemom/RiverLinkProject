using ArabRiver.Repository.Models;

namespace ArabRiver.Repository.Interfaces
{
    public interface IContactMessageRepository
    {
        Task<IEnumerable<ContactMessage>> GetAllAsync();

        Task<ContactMessage?> GetByIdAsync(Guid id);

        Task AddAsync(ContactMessage contactMessage);

        void Update(ContactMessage contactMessage);

        void Delete(ContactMessage contactMessage);

        Task SaveChangesAsync();
    }
}
