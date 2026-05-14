using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Models;
using ArabRiver.Service.DTOs.Contact;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Pagination;
using ArabRiver.Service.Responses;
using AutoMapper;

namespace ArabRiver.Service.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactMessageRepository
            _contactRepository;

        private readonly IEmailService
            _emailService;

        private readonly IMapper _mapper;

        public ContactService(
            IContactMessageRepository contactRepository,
            IEmailService emailService,
            IMapper mapper)
        {
            _contactRepository = contactRepository;

            _emailService = emailService;

            _mapper = mapper;
        }

        public async Task<ApiResponse<string>>
            CreateMessageAsync(
                CreateContactMessageDto dto,
                string ipAddress)
        {
            // Create entity

            var message =
                _mapper.Map<ContactMessage>(dto);

            message.Id = Guid.NewGuid();

            message.IpAddress = ipAddress;

            message.Status = "New";

            message.CreatedAt = DateTime.UtcNow;

            // Save in DB first

            await _contactRepository
                .AddAsync(message);

            await _contactRepository
                .SaveChangesAsync();

            // Send company notification

            await _emailService
                .SendContactNotificationAsync(
                    dto.Name,
                    dto.Email,
                    dto.Message);

            // Send auto reply

            await _emailService
                .SendAutoReplyAsync(
                    dto.Email,
                    dto.Name);

            return new ApiResponse<string>(
                true,
                "Message sent successfully");
        }

        public async Task<PagedResponse<ContactMessageResponseDto>>
            GetAllMessagesAsync(
                PaginationParameters parameters)
        {
            var messages =
                await _contactRepository
                    .GetAllAsync();

            var totalCount = messages.Count();

            var pagedMessages = messages
                .Skip((parameters.PageNumber - 1)
                    * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var mappedMessages =
                _mapper.Map<
                    IEnumerable<ContactMessageResponseDto>>(
                    pagedMessages);

            return new PagedResponse<
                ContactMessageResponseDto>(
                mappedMessages,
                parameters.PageNumber,
                parameters.PageSize,
                totalCount);
        }

        public Task<IEnumerable<ContactMessage>> GetUnreadAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>>
            MarkAsReadAsync(Guid id)
        {
            var message =
                await _contactRepository
                    .GetByIdAsync(id);

            if (message is null)
            {
                return new ApiResponse<string>(
                    false,
                    "Message not found");
            }

            message.Status = "Read";

            message.ReadAt = DateTime.UtcNow;

            _contactRepository.Update(message);

            await _contactRepository
                .SaveChangesAsync();

            return new ApiResponse<string>(
                true,
                "Message marked as read");
        }
    }
}
