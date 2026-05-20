using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetContactMessageById
{
    /// <summary>
    /// Retrieves a single contact message by ID. Pure query — no side effects.
    /// </summary>
    public record GetContactMessageByIdQuery(int Id) 
        : IRequest<ContactMessageDto?>;
}
