using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetContactMessageById
{
    /// <summary>
    /// Retrieves a single contact message by ID. Pure query — no side effects.
    /// If RequestingUserId is provided, the handler validates ownership.
    /// </summary>
    public record GetContactMessageByIdQuery(int Id, int? RequestingUserId = null) 
        : IRequest<ContactMessageDto?>;
}
