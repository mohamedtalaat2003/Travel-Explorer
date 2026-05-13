using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.MarkAsRead
{
    public record MarkAsReadCommand(int Id) : IRequest<ContactMessageDto>;
}
