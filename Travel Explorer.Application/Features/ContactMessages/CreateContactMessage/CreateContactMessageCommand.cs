using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage
{
    public record CreateContactMessageCommand(
        string FullName,
        string Email,
        string Subject,
        string Message 
    ) : IRequest<ContactMessageDto>;
}
