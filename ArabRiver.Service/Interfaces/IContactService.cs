using ArabRiver.Repository.Models;
using ArabRiver.Service.DTOs.Contact;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;

namespace ArabRiver.Service.Interfaces
{
    public interface IContactService
    {
        Task<ApiResponse<string>>
            CreateMessageAsync(
                CreateContactMessageDto dto,
                string ipAddress);

        Task<PagedResponse<ContactMessageResponseDto>>
            GetAllMessagesAsync(
                PaginationParameters parameters);

        Task<ApiResponse<string>>
            MarkAsReadAsync(Guid id);

        Task<IEnumerable<ContactMessage>>
            GetUnreadAsync();
    }
}
