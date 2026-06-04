using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetContactMessageById
{
    
    
    
    public record GetContactMessageByIdQuery(int Id) 
        : IRequest<ContactMessageDto?>;
}
