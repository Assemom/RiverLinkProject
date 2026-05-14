namespace ArabRiver.Service.DTOs.Contact
{
    public class ContactMessageResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
