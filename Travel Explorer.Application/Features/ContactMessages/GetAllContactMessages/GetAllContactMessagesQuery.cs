using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetAllContactMessages
{
    public record GetAllContactMessagesQuery(ContactMessageSpecParams Params) 
        : IRequest<PaginatedResult<ContactMessageDto>>;
}
